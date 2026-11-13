using Agroflet.Platform.Shared.Application.Internal.EventHandlers;
using Agroflet.Platform.Shipments.Domain.Model.Events;
using Agroflet.Platform.Shipments.Domain.Model.Aggregates;
using Agroflet.Platform.Shipments.Domain.Model.Commands;
using Agroflet.Platform.Shipments.Domain.Repositories;
namespace Agroflet.Platform.Shipments.Application.Internal.EventHandlers;

public class ShipmentLifecycleChangedEventHandler(IStatusChangeRepository history, ILogger<ShipmentLifecycleChangedEventHandler> logger) : IEventHandler<ShipmentLifecycleChangedEvent>
{
    public async Task Handle(ShipmentLifecycleChangedEvent e, CancellationToken ct)
    {
        await history.AddAsync(new StatusChange(new CreateStatusChangeCommand(e.ShipmentId.Value, e.PreviousStatus, e.Status, e.ChangedAt, e.DispatcherId.Value, e.Reason)), ct);
        logger.LogInformation("Shipment {ShipmentId} transition {Status} staged", e.ShipmentId.Value, e.Status);
    }
}
