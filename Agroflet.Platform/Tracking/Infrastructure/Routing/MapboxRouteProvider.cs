using System.Globalization;
using System.Text.Json;
using Agroflet.Platform.Tracking.Application.CommandServices;
using Agroflet.Platform.Tracking.Application.QueryServices;
using Agroflet.Platform.Tracking.Application.Internal.OutboundServices;
namespace Agroflet.Platform.Tracking.Infrastructure.Routing;

public class MapboxRouteProvider(HttpClient http, IConfiguration config) : IRouteProvider
{
    public async Task<PlannedRouteResult> Get(double originLatitude, double originLongitude, double destinationLatitude, double destinationLongitude, CancellationToken ct)
    {
        PlannedRouteResult Unavailable(string reason) => new("unavailable", "mapbox", null, null, null, reason);
        var token = config["Mapbox:AccessToken"]; if (string.IsNullOrWhiteSpace(token)) return Unavailable("provider-not-configured");
        var coordinates = FormattableString.Invariant($"{originLongitude},{originLatitude};{destinationLongitude},{destinationLatitude}");
        using var timeout = CancellationTokenSource.CreateLinkedTokenSource(ct); timeout.CancelAfter(TimeSpan.FromSeconds(Math.Clamp(config.GetValue<int>("Mapbox:TimeoutSeconds", 8), 1, 30)));
        try
        {
            using var response = await http.GetAsync("https://api.mapbox.com/directions/v5/mapbox/driving/" + coordinates + "?geometries=geojson&overview=full&access_token=" + Uri.EscapeDataString(token), timeout.Token);
            if (!response.IsSuccessStatusCode) return Unavailable("provider-unavailable");
            using var json = JsonDocument.Parse(await response.Content.ReadAsStringAsync(timeout.Token));
            if (json.RootElement.GetProperty("code").GetString() != "Ok" || json.RootElement.GetProperty("routes").GetArrayLength() == 0) return Unavailable("no-route");
            var route = json.RootElement.GetProperty("routes")[0]; var geometry = route.GetProperty("geometry");
            if (geometry.GetProperty("type").GetString() != "LineString" || geometry.GetProperty("coordinates").GetArrayLength() < 2) return Unavailable("invalid-provider-response");
            return new("available", "mapbox", geometry.Clone(), route.GetProperty("distance").GetDouble(), route.GetProperty("duration").GetDouble());
        }
        catch (OperationCanceledException) when (!ct.IsCancellationRequested) { return Unavailable("provider-timeout"); }
        catch (Exception ex) when (ex is HttpRequestException or JsonException or KeyNotFoundException or InvalidOperationException) { return Unavailable("provider-unavailable"); }
    }
}
