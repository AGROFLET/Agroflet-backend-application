using System.Text.Json;
namespace Agroflet.Platform.Tracking.Application.Internal.OutboundServices;

public record PlannedRouteResult(string Status, string Provider, JsonElement? Geometry, double? DistanceMeters, double? DurationSeconds, string? Reason = null);
public interface IRouteProvider { Task<PlannedRouteResult> Get(double originLatitude, double originLongitude, double destinationLatitude, double destinationLongitude, CancellationToken ct); }
