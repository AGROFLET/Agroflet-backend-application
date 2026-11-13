using Agroflet.Platform.Shared.Application.Internal.EventHandlers;
using Agroflet.Platform.Shipments.Domain.Model.Events;
using Agroflet.Platform.Incidents.Application.CommandServices;
using Agroflet.Platform.Incidents.Application.QueryServices;
using Agroflet.Platform.Incidents.Application.Internal.OutboundServices;
namespace Agroflet.Platform.Incidents.Application.Internal.EventHandlers;

public class ShipmentNotificationEventHandler(INotificationPublisher notifications, ILogger<ShipmentNotificationEventHandler> logger) : IEventHandler<ShipmentLifecycleChangedEvent>
{
    public async Task Handle(ShipmentLifecycleChangedEvent e, CancellationToken ct)
    {
        await notifications.Publish(e.ShipmentId.Value, e.Code, e.DispatcherId.Value, e.BuyerId.Value, "status_change", $"shipment-{e.ShipmentId.Value}-{e.Status}", new { previousStatus = e.PreviousStatus, newStatus = e.Status }, ct);
        logger.LogInformation("Participant notifications for shipment {ShipmentId} staged", e.ShipmentId.Value);
    }
}
