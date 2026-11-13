using System.Net;
using System.Net.Http.Json;
using Agroflet.Platform.Iam.Application.CommandServices;
using Agroflet.Platform.Iam.Domain.Model.Commands;
using Agroflet.Platform.Iam.Domain.Model.Aggregates;
using Agroflet.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using Agroflet.Platform.Tracking.Application.Internal.OutboundServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Xunit;

namespace Agroflet.Platform.Tests;

public partial class ApiTests
{
    private sealed class CountingRoutes : IRouteProvider
    {
        public int Calls { get; private set; }
        public Task<PlannedRouteResult> Get(double a, double b, double c, double d, CancellationToken ct)
        {
            Calls++;
            return Task.FromResult(new PlannedRouteResult("unavailable", "test", null, null, null));
        }
    }

    [Fact]
    public async Task Route_acl_rejects_other_users_before_contacting_provider()
    {
        var routes = new CountingRoutes();
        using var factory = new ApiFactory(services => services.Replace(ServiceDescriptor.Singleton<IRouteProvider>(routes)));
        var (owner, ownerId) = await Account(factory, "dispatcher", "owner@example.com");
        var (buyer, buyerId) = await Account(factory, "buyer", "buyer@example.com");
        var (other, _) = await Account(factory, "buyer", "other@example.com");
        var (vehicle, driver) = await Fleet(owner, ownerId);
        var shipment = await Json(await owner.PostAsJsonAsync("/api/v1/shipments", Draft(buyerId, vehicle, driver)));
        var id = shipment.GetProperty("id").GetInt32();
        Assert.Equal(HttpStatusCode.NotFound, (await other.GetAsync($"/api/v1/shipments/{id}/planned-route")).StatusCode);
        Assert.Equal(0, routes.Calls);
        Assert.Equal(HttpStatusCode.OK, (await buyer.GetAsync($"/api/v1/shipments/{id}/planned-route")).StatusCode);
        Assert.Equal(1, routes.Calls);
    }

    [Fact]
    public async Task Application_returns_failure_result_without_persisting_invalid_user()
    {
        using var factory = new ApiFactory();
        using var scope = factory.Services.CreateScope();
        var service = scope.ServiceProvider.GetRequiredService<IAuthenticationCommandService>();
        var result = await service.Handle(new SignUpCommand("", "Test", "valid@example.com", "Agroflet2026!", "buyer"), default);
        Assert.True(result.IsFailure);
        Assert.Equal(400, result.Error!.Status);
        Assert.Null(result.Value);
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        Assert.Equal(0, await db.Set<User>().CountAsync());
    }

    [Fact]
    public async Task Buyer_projection_hides_driver_identity_and_cancellation_preserves_history()
    {
        using var factory = new ApiFactory();
        var (owner, ownerId) = await Account(factory, "dispatcher", "owner@example.com");
        var (buyer, buyerId) = await Account(factory, "buyer", "buyer@example.com");
        var (vehicle, driver) = await Fleet(owner, ownerId);
        var shipment = await Json(await owner.PostAsJsonAsync("/api/v1/shipments", Draft(buyerId, vehicle, driver)));
        var id = shipment.GetProperty("id").GetInt32();
        var visibleDriver = await Json(await buyer.GetAsync($"/api/v1/drivers/{driver}"));
        Assert.True(string.IsNullOrEmpty(visibleDriver.GetProperty("dni").GetString()));
        var response = await owner.PatchAsJsonAsync($"/api/v1/shipments/{id}/status", new
        {
            status = "cancelled",
            cancellationReason = "Cancelled by owner",
            expectedVersion = shipment.GetProperty("version").GetInt32()
        });
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(HttpStatusCode.MethodNotAllowed, (await owner.DeleteAsync($"/api/v1/shipments/{id}")).StatusCode);
        Assert.Equal("cancelled", (await Json(await buyer.GetAsync($"/api/v1/shipments/{id}"))).GetProperty("status").GetString());
        Assert.Equal(2, (await Json(await buyer.GetAsync($"/api/v1/shipments/{id}/status-history"))).GetArrayLength());
    }
}
