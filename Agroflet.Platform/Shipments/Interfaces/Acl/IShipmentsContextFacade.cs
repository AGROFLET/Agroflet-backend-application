namespace Agroflet.Platform.Shipments.Interfaces.Acl;

public record ShipmentContextResource(int Id, string Code, int DispatcherId, int BuyerId, string Status);
public record RouteEndpoints(double OriginLatitude, double OriginLongitude, double DestinationLatitude, double DestinationLongitude);
public interface IShipmentsContextFacade
{
    Task<RouteEndpoints> GetRouteEndpoints(int shipmentId, int participantId, CancellationToken ct);
    Task<ShipmentContextResource> RequireOwned(int shipmentId, int dispatcherId, CancellationToken ct);
    Task<ShipmentContextResource> RequireOwnedTransit(int shipmentId, int dispatcherId, CancellationToken ct);
    Task<ShipmentContextResource> ApplyIncidentDelay(int shipmentId, int dispatcherId, int minutes, CancellationToken ct);
    Task TouchTransit(int shipmentId, int dispatcherId, CancellationToken ct);
}
