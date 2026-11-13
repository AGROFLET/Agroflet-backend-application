
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Agroflet.Platform.Contact.Domain.Model;
using Agroflet.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using Agroflet.Platform.Tracking.Infrastructure.Routing;
using Xunit;
namespace Agroflet.Platform.Tests;

public partial class ApiTests
{
    [Fact]
    public async Task Logout_revokes_only_current_session_and_profile_aliases_work()
    {
        using var f = new ApiFactory(); var (a, id) = await Account(f, "buyer", "logout@example.com");
        var second = f.CreateClient(); var login = await Json(await second.PostAsJsonAsync("/api/v1/auth/login", new { email = "logout@example.com", password = "Agroflet2026!" }));
        second.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", login.GetProperty("accessToken").GetString());
        var profile = await Json(await a.GetAsync("/api/v1/users/profile")); Assert.Equal(id, profile.GetProperty("id").GetInt32());
        await Json(await a.PutAsJsonAsync("/api/v1/users/profile", new { preferredLanguage = "es_419" }));
        Assert.Equal(HttpStatusCode.NoContent, (await a.PostAsync("/api/v1/auth/logout", null)).StatusCode);
        Assert.Equal(HttpStatusCode.Unauthorized, (await a.GetAsync("/api/v1/users/profile")).StatusCode);
        Assert.Equal(HttpStatusCode.OK, (await second.GetAsync("/api/v1/users/profile")).StatusCode);
    }
    [Fact]
    public async Task Report_contracts_validate_pagination_versions_and_position_retries()
    {
        using var f = new ApiFactory(); var (d, id) = await Account(f, "dispatcher", "owner@example.com"); var (b, bid) = await Account(f, "buyer", "buyer@example.com"); var (x, _) = await Account(f, "buyer", "other@example.com"); var (v, driver) = await Fleet(d, id);
        var s = await Json(await d.PostAsJsonAsync("/api/v1/shipments", Draft(bid, v, driver))); var sid = s.GetProperty("id").GetInt32(); var version = s.GetProperty("version").GetInt32();
        var page = await Json(await d.GetAsync("/api/v1/shipments?pageSize=1&search=ABC-123")); Assert.Equal(1, page.GetProperty("totalRecords").GetInt32()); Assert.Single(page.GetProperty("data").EnumerateArray());
        Assert.Equal(0, (await Json(await x.GetAsync("/api/v1/shipments"))).GetProperty("totalRecords").GetInt32());
        Assert.Equal(HttpStatusCode.BadRequest, (await d.GetAsync("/api/v1/shipments?pageSize=0")).StatusCode);
        Assert.Equal(HttpStatusCode.BadRequest, (await d.GetAsync("/api/v1/shipments?startDate=2026-11-09&endDate=2026-11-01")).StatusCode);
        var locations = await Json(await b.GetAsync("/api/v1/shipments/active/locations")); Assert.False(locations[0].GetProperty("hasPosition").GetBoolean()); Assert.Equal(JsonValueKind.Null, locations[0].GetProperty("latitude").ValueKind);
        await Json(await d.PatchAsJsonAsync($"/api/v1/shipments/{sid}/status", new { status = "in_transit", expectedVersion = version }));
        Assert.Equal(HttpStatusCode.Conflict, (await d.PatchAsJsonAsync($"/api/v1/shipments/{sid}/status", new { status = "delivered", expectedVersion = version })).StatusCode);
        var at = DateTime.UtcNow; var position = new { latitude = -12.1, longitude = -75.2, reportedAt = at, source = "driver_call", note = "report", eventId = "retry-1" };
        var p = await Json(await d.PostAsJsonAsync($"/api/v1/shipments/{sid}/positions", position)); var retry = await Json(await d.PostAsJsonAsync($"/api/v1/shipments/{sid}/positions", position)); Assert.Equal(p.GetProperty("id").GetInt32(), retry.GetProperty("id").GetInt32());
        Assert.Equal(HttpStatusCode.Conflict, (await d.PostAsJsonAsync($"/api/v1/shipments/{sid}/positions", new { latitude = -13.0, longitude = -75.2, reportedAt = at, source = "driver_call", note = "report", eventId = "retry-1" })).StatusCode);
        Assert.Single((await Json(await b.GetAsync($"/api/v1/shipments/{sid}/positions"))).EnumerateArray());
        Assert.Equal(HttpStatusCode.NotFound, (await x.GetAsync($"/api/v1/shipments/{sid}/positions")).StatusCode);
        Assert.Equal(2, (await Json(await b.GetAsync($"/api/v1/shipments/{sid}/status-history"))).GetArrayLength());
        var incident = await Json(await d.PostAsJsonAsync($"/api/v1/shipments/{sid}/incidents", new { type = "road_block", description = "Delay", estimatedDelayMinutes = 10, occurredAt = at, idempotencyKey = "incident-1" })); Assert.True(incident.TryGetProperty("updatedEstimatedArrival", out _));
        var notifications = await Json(await b.GetAsync("/api/v1/notifications")); var nid = notifications[0].GetProperty("id").GetInt32();
        var read = await Json(await b.PatchAsJsonAsync($"/api/v1/notifications/{nid}/read", new { })); var readAgain = await Json(await b.PatchAsJsonAsync($"/api/v1/notifications/{nid}/read", new { })); Assert.Equal(read.GetProperty("readAt").GetString(), readAgain.GetProperty("readAt").GetString());
        var route = await Json(await b.GetAsync($"/api/v1/shipments/{sid}/planned-route")); Assert.Equal("unavailable", route.GetProperty("status").GetString()); Assert.Equal(JsonValueKind.Null, route.GetProperty("geometry").ValueKind);
        Assert.Equal(HttpStatusCode.NotFound, (await x.GetAsync($"/api/v1/shipments/{sid}/planned-route")).StatusCode);
    }
    [Fact]
    public async Task Contact_is_persisted_and_never_exposed_as_public_collection()
    {
        using var f = new ApiFactory(); var c = f.CreateClient(); var response = await c.PostAsJsonAsync("/api/v1/contact-messages", new { name = "Visitor", email = "visitor@example.com", message = "Please contact me" }); Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        var receipt = await Json(response); Assert.False(receipt.TryGetProperty("email", out _));
        using var scope = f.Services.CreateScope(); var db = scope.ServiceProvider.GetRequiredService<AppDbContext>(); Assert.Equal(1, await db.Set<ContactMessage>().CountAsync());
        Assert.Equal(HttpStatusCode.MethodNotAllowed, (await c.GetAsync("/api/v1/contact-messages")).StatusCode);
        Assert.Equal(HttpStatusCode.BadRequest, (await c.PostAsJsonAsync("/api/v1/contact-messages", new { name = "Visitor", email = "bad", message = "Hi" })).StatusCode);
    }
    private sealed class RouteHttpHandler(string body, HttpStatusCode status = HttpStatusCode.OK) : HttpMessageHandler
    { protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken ct) => Task.FromResult(new HttpResponseMessage(status) { Content = new StringContent(body) }); }
    [Theory]
    [InlineData("{\"code\":\"Ok\",\"routes\":[{\"geometry\":{\"type\":\"LineString\",\"coordinates\":[[-75,-12],[-76,-12]]},\"distance\":150000,\"duration\":7200}]}", "available")]
    [InlineData("{\"code\":\"NoRoute\",\"routes\":[]}", "unavailable")]
    [InlineData("invalid json", "unavailable")]
    public async Task Mapbox_adapter_parses_routes_and_never_invents_a_route(string body, string expected)
    {
        var config = new ConfigurationBuilder().AddInMemoryCollection(new Dictionary<string, string?> { { "Mapbox:AccessToken", "test-token" } }).Build();
        var provider = new MapboxRouteProvider(new HttpClient(new RouteHttpHandler(body)), config); var route = await provider.Get(-12, -75, -12, -76, default); Assert.Equal(expected, route.Status); Assert.Equal(expected == "available", route.Geometry.HasValue);
    }
}
