using Agroflet.Platform.Shipments.Domain.Repositories;
using Agroflet.Platform.Shipments.Domain.Model.Aggregates;
using Agroflet.Platform.Shipments.Interfaces.Acl;
namespace Agroflet.Platform.Shipments.Application.Acl;

public class ShipmentsContextFacade(IShipmentRepository shipments, ILocationRepository locations) : IShipmentsContextFacade
{
    public async Task<RouteEndpoints> GetRouteEndpoints(int shipmentId, int participantId, CancellationToken ct)
    {
        var shipment = await shipments.FindByIdAsync(shipmentId, ct) ?? throw new BusinessException("not-found", 404);
        if (shipment.DispatcherId != participantId && shipment.BuyerId != participantId) throw new BusinessException("not-found", 404);
        var origin = await locations.FindByIdAsync(shipment.OriginId, ct) ?? throw new BusinessException("not-found", 404);
        var destination = await locations.FindByIdAsync(shipment.DestinationId, ct) ?? throw new BusinessException("not-found", 404);
        return new(origin.Latitude, origin.Longitude, destination.Latitude, destination.Longitude);
    }
    private async Task<Shipment> Find(int id, int dispatcherId, CancellationToken ct)
    {
        var shipment = await shipments.FindByIdAsync(id, ct) ?? throw new BusinessException("not-found", 404);
        if (shipment.DispatcherId != dispatcherId) throw new BusinessException("not-found", 404);
        return shipment;
    }
    private static ShipmentContextResource Resource(Shipment s) => new(s.Id, s.Code, s.DispatcherId, s.BuyerId, s.Status);
    public async Task<ShipmentContextResource> RequireOwned(int shipmentId, int dispatcherId, CancellationToken ct)
        => Resource(await Find(shipmentId, dispatcherId, ct));
    public async Task<ShipmentContextResource> RequireOwnedTransit(int shipmentId, int dispatcherId, CancellationToken ct)
    {
        var shipment = await Find(shipmentId, dispatcherId, ct);
        if (shipment.Status != "in_transit") throw new BusinessException("not-in-transit", 422);
        return Resource(shipment);
    }
    public async Task<ShipmentContextResource> ApplyIncidentDelay(int shipmentId, int dispatcherId, int minutes, CancellationToken ct)
    {
        await RequireOwnedTransit(shipmentId, dispatcherId, ct);
        var shipment = await Find(shipmentId, dispatcherId, ct);
        shipment.Delay(minutes);
        return Resource(shipment);
    }
    public async Task TouchTransit(int shipmentId, int dispatcherId, CancellationToken ct)
    {
        await RequireOwnedTransit(shipmentId, dispatcherId, ct);
        (await Find(shipmentId, dispatcherId, ct)).Touch();
    }
}
