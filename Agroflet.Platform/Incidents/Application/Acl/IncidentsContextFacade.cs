using Agroflet.Platform.Incidents.Application.CommandServices;
using Agroflet.Platform.Incidents.Application.QueryServices;
using Agroflet.Platform.Incidents.Application.Internal.OutboundServices;
using Agroflet.Platform.Incidents.Interfaces.Acl;

namespace Agroflet.Platform.Incidents.Application.Acl;

public class IncidentsContextFacade(INotificationPublisher notifications) : IIncidentsContextFacade
{
    public Task NotifyShipmentStatus(int shipmentId, string code, int dispatcherId, int buyerId,
        int statusChangeId, string? previousStatus, string status, CancellationToken ct)
        => notifications.Publish(shipmentId, code, dispatcherId, buyerId, "status_change",
            $"status-change-{statusChangeId}", new { previousStatus, newStatus = status }, ct);
}
