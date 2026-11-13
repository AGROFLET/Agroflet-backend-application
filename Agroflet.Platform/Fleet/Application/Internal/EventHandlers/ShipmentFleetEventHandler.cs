using Agroflet.Platform.Shared.Application.Internal.EventHandlers;
using Agroflet.Platform.Shipments.Domain.Model.Events;
using Agroflet.Platform.Fleet.Interfaces.Acl;
namespace Agroflet.Platform.Fleet.Application.Internal.EventHandlers;

public class ShipmentFleetEventHandler(IFleetContextFacade fleet, ILogger<ShipmentFleetEventHandler> logger) : IEventHandler<ShipmentLifecycleChangedEvent>
{
    public async Task Handle(ShipmentLifecycleChangedEvent e, CancellationToken ct)
    {
        await fleet.ApplyShipmentState(e.VehicleId.Value, e.DriverId.Value, e.DispatcherId.Value, e.Status, ct);
        logger.LogInformation("Fleet allocation for shipment {ShipmentId} staged", e.ShipmentId.Value);
    }
}
