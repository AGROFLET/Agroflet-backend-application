using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using Agroflet.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using Agroflet.Platform.Fleet.Domain.Model.Aggregates;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Agroflet.Platform.Incidents.Application.CommandServices;
using Agroflet.Platform.Incidents.Application.QueryServices;
using Agroflet.Platform.Incidents.Application.Internal.OutboundServices;
using Microsoft.Extensions.DependencyInjection.Extensions;
namespace Agroflet.Platform.Tests;

public partial class ApiTests
{
    private sealed class FailingNotifications : INotificationPublisher
    {
        public Task Publish(int shipmentId, string code, int dispatcherId, int buyerId, string type,
            string key, object payload, CancellationToken ct) => throw new InvalidOperationException("Injected handler failure");
    }

    private sealed class ConditionalNotifications(INotificationPublisher inner, Func<bool> shouldFail) : INotificationPublisher
    {
        public Task Publish(int shipmentId, string code, int dispatcherId, int buyerId, string type,
            string key, object payload, CancellationToken ct)
            => shouldFail() ? throw new InvalidOperationException("Injected incident handler failure")
                : inner.Publish(shipmentId, code, dispatcherId, buyerId, type, key, payload, ct);
    }

    [Fact]
    public async Task Incident_handler_failure_rolls_back_delay_and_allows_safe_retry()
    {
        bool fail = false;
        using var factory = new ApiFactory(services =>
        {
            services.AddScoped<Agroflet.Platform.Incidents.Application.Internal.CommandServices.NotificationPublisher>();
            services.Replace(ServiceDescriptor.Scoped<INotificationPublisher>(provider => new ConditionalNotifications(
                provider.GetRequiredService<Agroflet.Platform.Incidents.Application.Internal.CommandServices.NotificationPublisher>(), () => fail)));
        });
        var (client, owner) = await Account(factory, "dispatcher", "owner@example.com");
        var (_, buyer) = await Account(factory, "buyer", "buyer@example.com");
        var (vehicle, driver) = await Fleet(client, owner);
        var shipment = await Json(await client.PostAsJsonAsync("/api/v1/shipments", Draft(buyer, vehicle, driver)));
        int id = shipment.GetProperty("id").GetInt32();
        await Json(await client.PatchAsJsonAsync($"/api/v1/shipments/{id}", new { status = "in_transit" }));
        var incident = new
        {
            shipmentId = id,
            type = "road_block",
            description = "Bloqueo",
            estimatedDelayMinutes = 30,
            occurredAt = DateTime.UtcNow,
            idempotencyKey = "rollback-and-retry"
        };
        fail = true;
        Assert.Equal(HttpStatusCode.InternalServerError, (await client.PostAsJsonAsync("/api/v1/incidents", incident)).StatusCode);
        var afterFailure = await Json(await client.GetAsync($"/api/v1/shipments/{id}"));
        Assert.Equal(shipment.GetProperty("estimatedArrivalAt").GetDateTime(), afterFailure.GetProperty("estimatedArrivalAt").GetDateTime());
        Assert.Empty((await Json(await client.GetAsync($"/api/v1/incidents?shipmentId={id}"))).EnumerateArray());
        fail = false;
        await Json(await client.PostAsJsonAsync("/api/v1/incidents", incident));
        var afterRetry = await Json(await client.GetAsync($"/api/v1/shipments/{id}"));
        Assert.Equal(shipment.GetProperty("estimatedArrivalAt").GetDateTime().AddMinutes(30), afterRetry.GetProperty("estimatedArrivalAt").GetDateTime());
    }

    [Fact]
    public async Task Handler_failure_rolls_back_shipment_reservation_and_history()
    {
        using var factory = new ApiFactory(services => services.Replace(
            ServiceDescriptor.Scoped<INotificationPublisher>(_ => new FailingNotifications())));
        var (client, owner) = await Account(factory, "dispatcher", "owner@example.com");
        var (_, buyer) = await Account(factory, "buyer", "buyer@example.com");
        var (vehicle, driver) = await Fleet(client, owner);
        Assert.Equal(HttpStatusCode.InternalServerError,
            (await client.PostAsJsonAsync("/api/v1/shipments", Draft(buyer, vehicle, driver))).StatusCode);
        Assert.Empty((await Json(await client.GetAsync("/api/v1/shipments"))).GetProperty("data").EnumerateArray());
        Assert.Empty((await Json(await client.GetAsync("/api/v1/status-changes"))).EnumerateArray());
        Assert.Equal("available", (await Json(await client.GetAsync($"/api/v1/vehicles/{vehicle}"))).GetProperty("status").GetString());
        Assert.Equal("available", (await Json(await client.GetAsync($"/api/v1/drivers/{driver}"))).GetProperty("status").GetString());
    }

    [Fact]
    public async Task Incident_retry_after_delivery_preserves_eta_and_notifications()
    {
        using var factory = new ApiFactory();
        var (client, owner) = await Account(factory, "dispatcher", "owner@example.com");
        var (_, buyer) = await Account(factory, "buyer", "buyer@example.com");
        var (vehicle, driver) = await Fleet(client, owner);
        var shipment = await Json(await client.PostAsJsonAsync("/api/v1/shipments", Draft(buyer, vehicle, driver)));
        int id = shipment.GetProperty("id").GetInt32();
        await Json(await client.PatchAsJsonAsync($"/api/v1/shipments/{id}", new { status = "in_transit" }));
        var incident = new
        {
            shipmentId = id,
            type = "road_block",
            description = "Bloqueo",
            estimatedDelayMinutes = 20,
            occurredAt = DateTime.UtcNow,
            idempotencyKey = "retry-after-delivery"
        };
        var first = await Json(await client.PostAsJsonAsync("/api/v1/incidents", incident));
        await Json(await client.PatchAsJsonAsync($"/api/v1/shipments/{id}", new { status = "delivered" }));
        var retry = await Json(await client.PostAsJsonAsync("/api/v1/incidents", incident));
        Assert.Equal(first.GetProperty("id").GetInt32(), retry.GetProperty("id").GetInt32());
        var final = await Json(await client.GetAsync($"/api/v1/shipments/{id}"));
        Assert.Equal(shipment.GetProperty("estimatedArrivalAt").GetDateTime().AddMinutes(20), final.GetProperty("estimatedArrivalAt").GetDateTime());
        Assert.Equal(4, (await Json(await client.GetAsync("/api/v1/notifications"))).GetArrayLength());
    }

    [Fact]
    public async Task Problem_details_localize_without_changing_error_code()
    {
        using var factory = new ApiFactory();
        var client = factory.CreateClient();
        async Task<JsonElement> Failure(string language)
        {
            using var request = new HttpRequestMessage(HttpMethod.Post, "/api/v1/authentication/sign-in");
            request.Headers.AcceptLanguage.ParseAdd(language);
            request.Content = JsonContent.Create(new { email = "unknown@example.com", password = "WrongPassword!" });
            var response = await client.SendAsync(request);
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
            Assert.Equal("application/problem+json", response.Content.Headers.ContentType?.MediaType);
            return JsonDocument.Parse(await response.Content.ReadAsStringAsync()).RootElement.Clone();
        }
        var es = await Failure("es"); var en = await Failure("en");
        Assert.Equal("invalid-credentials", es.GetProperty("code").GetString());
        Assert.Equal(es.GetProperty("code").GetString(), en.GetProperty("code").GetString());
        Assert.Equal("Correo o contraseña incorrectos.", es.GetProperty("detail").GetString());
        Assert.Equal("Invalid email or password.", en.GetProperty("detail").GetString());
    }

    [Fact]
    public async Task Bcrypt_preserves_password_suffix_beyond_72_bytes()
    {
        using var factory = new ApiFactory(); var client = factory.CreateClient();
        string prefix = new('a', 80); string password = prefix + "1!";
        await Json(await client.PostAsJsonAsync("/api/v1/authentication/sign-up", new
        {
            firstName = "Rosa",
            lastName = "Quispe",
            email = "long@example.com",
            password,
            userType = "buyer"
        }));
        await Json(await client.PostAsJsonAsync("/api/v1/authentication/sign-in", new { email = "long@example.com", password }));
        Assert.Equal(HttpStatusCode.Unauthorized, (await client.PostAsJsonAsync("/api/v1/authentication/sign-in",
            new { email = "long@example.com", password = prefix + "2!" })).StatusCode);
    }
    private static async Task<JsonElement> Json(HttpResponseMessage response)
    {
        var body = await response.Content.ReadAsStringAsync();
        Assert.True(response.IsSuccessStatusCode, $"{response.StatusCode}: {body}"); return JsonDocument.Parse(body).RootElement.Clone();
    }
    private static async Task<(HttpClient Client, int Id)> Account(ApiFactory factory, string role, string email)
    {
        var client = factory.CreateClient();
        var created = await Json(await client.PostAsJsonAsync("/api/v1/authentication/sign-up", new { firstName = "Rosa", lastName = "Quispe", email, password = "Agroflet2026!", userType = role, phone = "987654321", companyName = "Cooperativa", preferredLanguage = "es" }));
        Assert.False(created.TryGetProperty("passwordHash", out _)); Assert.False(created.TryGetProperty("password", out _));
        var session = await Json(await client.PostAsJsonAsync("/api/v1/authentication/sign-in", new { email, password = "Agroflet2026!" }));
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", session.GetProperty("accessToken").GetString());
        return (client, created.GetProperty("id").GetInt32());
    }
    private static async Task<(int Vehicle, int Driver)> Fleet(HttpClient client, int dispatcherId)
    {
        var vehicle = await Json(await client.PostAsJsonAsync("/api/v1/vehicles", new { dispatcherId, plate = "ABC-123", brand = "Volvo", model = "FM", year = 2020, capacityTons = 12, bodyType = "dry_van", status = "available" }));
        var driver = await Json(await client.PostAsJsonAsync("/api/v1/drivers", new { dispatcherId, firstName = "Luis", lastName = "Soto", dni = "12345678", licenseNumber = "A12345678", licenseCategory = "A-IIIb", phone = "987654321", status = "available" }));
        return (vehicle.GetProperty("id").GetInt32(), driver.GetProperty("id").GetInt32());
    }
    private static object Draft(int buyer, int vehicle, int driver, decimal weight = 5) => new { buyerId = buyer, cargoType = "potato", cargoDescription = "", weightTons = weight, originId = 1, destinationId = 7, vehicleId = vehicle, driverId = driver, plannedDepartureAt = DateTime.UtcNow.AddHours(1), estimatedArrivalAt = DateTime.UtcNow.AddHours(5) };
    [Fact]
    public async Task Lifecycle_is_atomic_scoped_and_incidents_are_idempotent()
    {
        using var factory = new ApiFactory();
        var (dispatcher, id) = await Account(factory, "dispatcher", "owner@example.com");
        var (buyer, buyerId) = await Account(factory, "buyer", "buyer@example.com");
        var (foreign, _) = await Account(factory, "dispatcher", "foreign@example.com");
        var (vehicle, driver) = await Fleet(dispatcher, id);
        var shipment = await Json(await dispatcher.PostAsJsonAsync("/api/v1/shipments", Draft(buyerId, vehicle, driver)));
        int shipmentId = shipment.GetProperty("id").GetInt32();
        Assert.Equal($"AGF-{shipmentId:D4}", shipment.GetProperty("code").GetString());
        Assert.Equal("reserved", (await Json(await dispatcher.GetAsync($"/api/v1/vehicles/{vehicle}"))).GetProperty("status").GetString());
        Assert.Equal(HttpStatusCode.Conflict, (await dispatcher.PostAsJsonAsync("/api/v1/shipments", Draft(buyerId, vehicle, driver))).StatusCode);
        Assert.Equal(HttpStatusCode.NotFound, (await foreign.GetAsync($"/api/v1/shipments/{shipmentId}")).StatusCode);
        Assert.Empty((await Json(await foreign.GetAsync($"/api/v1/shipments?dispatcherId={id}"))).GetProperty("data").EnumerateArray());
        Assert.Equal(HttpStatusCode.Forbidden, (await buyer.PatchAsJsonAsync($"/api/v1/shipments/{shipmentId}", new { status = "in_transit" })).StatusCode);
        Assert.Equal(HttpStatusCode.Conflict, (await dispatcher.PatchAsJsonAsync($"/api/v1/shipments/{shipmentId}", new { status = "delivered" })).StatusCode);
        await Json(await dispatcher.PatchAsJsonAsync($"/api/v1/shipments/{shipmentId}", new { status = "in_transit" }));
        Assert.Equal(HttpStatusCode.Conflict, (await dispatcher.PatchAsJsonAsync($"/api/v1/vehicles/{vehicle}", new { status = "available" })).StatusCode);
        var incident = new { shipmentId, type = "road_block", description = "DesvÃ­o", estimatedDelayMinutes = 30, occurredAt = DateTime.UtcNow, idempotencyKey = "one-incident" };
        var first = await Json(await dispatcher.PostAsJsonAsync("/api/v1/incidents", incident));
        var retry = await Json(await dispatcher.PostAsJsonAsync("/api/v1/incidents", incident));
        Assert.Equal(first.GetProperty("id").GetInt32(), retry.GetProperty("id").GetInt32());
        var after = await Json(await buyer.GetAsync($"/api/v1/shipments/{shipmentId}"));
        Assert.Equal(shipment.GetProperty("estimatedArrivalAt").GetDateTime().AddMinutes(30), after.GetProperty("estimatedArrivalAt").GetDateTime());
        await Json(await dispatcher.PostAsJsonAsync("/api/v1/positions", new { shipmentId, latitude = -12.1, longitude = -75.2, reportedAt = DateTime.UtcNow, source = "driver_call", note = "", isDemo = false }));
        Assert.Single((await Json(await buyer.GetAsync($"/api/v1/positions?shipmentId={shipmentId}"))).EnumerateArray());
        await Json(await buyer.GetAsync($"/api/v1/users/{id}"));
        await Json(await dispatcher.PatchAsJsonAsync($"/api/v1/shipments/{shipmentId}", new { status = "delivered" }));
        Assert.Equal("available", (await Json(await dispatcher.GetAsync($"/api/v1/vehicles/{vehicle}"))).GetProperty("status").GetString());
        Assert.Equal(3, (await Json(await buyer.GetAsync($"/api/v1/status-changes?shipmentId={shipmentId}"))).GetArrayLength());
        var notifications = await Json(await buyer.GetAsync("/api/v1/notifications")); Assert.Equal(4, notifications.GetArrayLength());
        int notificationId = notifications[0].GetProperty("id").GetInt32();
        Assert.Equal(HttpStatusCode.NotFound, (await foreign.PatchAsJsonAsync($"/api/v1/notifications/{notificationId}", new { isRead = true })).StatusCode);
        Assert.True((await Json(await buyer.PatchAsJsonAsync($"/api/v1/notifications/{notificationId}", new { isRead = true }))).GetProperty("isRead").GetBoolean());
    }
    [Fact]
    public async Task Validation_and_cancellation_preserve_resources()
    {
        using var factory = new ApiFactory(); var (client, id) = await Account(factory, "dispatcher", "owner@example.com"); var (_, buyer) = await Account(factory, "buyer", "buyer@example.com");
        var (vehicle, driver) = await Fleet(client, id);
        Assert.Equal(HttpStatusCode.UnprocessableEntity, (await client.PostAsJsonAsync("/api/v1/shipments", Draft(buyer, vehicle, driver, 100))).StatusCode);
        Assert.Equal("available", (await Json(await client.GetAsync($"/api/v1/vehicles/{vehicle}"))).GetProperty("status").GetString());
        var s = await Json(await client.PostAsJsonAsync("/api/v1/shipments", Draft(buyer, vehicle, driver))); var sid = s.GetProperty("id").GetInt32();
        Assert.Equal(HttpStatusCode.BadRequest, (await client.PatchAsJsonAsync($"/api/v1/shipments/{sid}", new { status = "cancelled", cancellationReason = "" })).StatusCode);
        await Json(await client.PatchAsJsonAsync($"/api/v1/shipments/{sid}", new { status = "cancelled", cancellationReason = "Pedido cancelado" }));
        Assert.Equal("available", (await Json(await client.GetAsync($"/api/v1/drivers/{driver}"))).GetProperty("status").GetString());
        Assert.Equal(HttpStatusCode.Conflict, (await client.PatchAsJsonAsync($"/api/v1/shipments/{sid}", new { status = "in_transit" })).StatusCode);
    }
    [Fact]
    public async Task Password_reset_is_single_use_and_revokes_tokens()
    {
        using var factory = new ApiFactory(); var (client, id) = await Account(factory, "buyer", "buyer@example.com");
        var anonymous = factory.CreateClient(); Assert.Equal(HttpStatusCode.Unauthorized, (await anonymous.GetAsync("/api/v1/shipments")).StatusCode);
        await Json(await client.PatchAsJsonAsync($"/api/v1/users/{id}", new { preferredLanguage = "en" }));
        var reset = await Json(await anonymous.PostAsJsonAsync("/api/v1/authentication/password-reset-requests", new { email = "buyer@example.com" }));
        var command = new { token = reset.GetProperty("token").GetString(), newPassword = "NewPassword2026!" };
        Assert.Equal(HttpStatusCode.NoContent, (await anonymous.PostAsJsonAsync("/api/v1/authentication/reset-password", command)).StatusCode);
        Assert.Equal(HttpStatusCode.BadRequest, (await anonymous.PostAsJsonAsync("/api/v1/authentication/reset-password", command)).StatusCode);
        Assert.Equal(HttpStatusCode.Unauthorized, (await client.GetAsync($"/api/v1/users/{id}")).StatusCode);
        Assert.Equal(HttpStatusCode.Unauthorized, (await anonymous.PostAsJsonAsync("/api/v1/authentication/sign-in", new { email = "buyer@example.com", password = "Agroflet2026!" })).StatusCode);
        await Json(await anonymous.PostAsJsonAsync("/api/v1/authentication/sign-in", new { email = "buyer@example.com", password = "NewPassword2026!" }));
    }
    [Fact]
    public async Task Optimistic_concurrency_rejects_stale_reservation()
    {
        using var factory = new ApiFactory(); var (client, id) = await Account(factory, "dispatcher", "owner@example.com"); var (vehicle, _) = await Fleet(client, id);
        using var firstScope = factory.Services.CreateScope(); using var secondScope = factory.Services.CreateScope();
        var first = firstScope.ServiceProvider.GetRequiredService<AppDbContext>(); var second = secondScope.ServiceProvider.GetRequiredService<AppDbContext>();
        var a = await first.Set<Vehicle>().SingleAsync(v => v.Id == vehicle); var b = await second.Set<Vehicle>().SingleAsync(v => v.Id == vehicle);
        a.SetOperationalStatus("reserved"); b.SetOperationalStatus("reserved"); await first.SaveChangesAsync();
        await Assert.ThrowsAsync<DbUpdateConcurrencyException>(() => second.SaveChangesAsync());
    }
    [Fact]
    public async Task OpenApi_and_health_are_available()
    {
        using var factory = new ApiFactory(); var client = factory.CreateClient();
        await Json(await client.GetAsync("/health/ready"));
        var api = await Json(await client.GetAsync("/openapi/v1.json")); Assert.True(api.GetProperty("paths").TryGetProperty("/api/v1/shipments", out _));
    }
}
