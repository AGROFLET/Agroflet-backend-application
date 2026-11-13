using Agroflet.Platform.Shared.Application.Internal.EventHandlers;
using Agroflet.Platform.Shipments.Domain.Model.Events;
using Agroflet.Platform.Incidents.Application.CommandServices;
using Agroflet.Platform.Incidents.Application.QueryServices;
using Agroflet.Platform.Incidents.Application.Internal.OutboundServices;
namespace Agroflet.Platform.Incidents.Application.Internal.EventHandlers;
/// <summary>Persists participant alerts after Shipments has applied an incident.</summary>
public class IncidentReportedEventHandler(INotificationPublisher notifications, ILogger<IncidentReportedEventHandler> logger) : IEventHandler<ShipmentEtaChangedEvent>
{
    public async Task Handle(ShipmentEtaChangedEvent e, CancellationToken ct)
    {
        await notifications.Publish(e.ShipmentId.Value, e.Code, e.DispatcherId.Value, e.BuyerId.Value, "incident", $"incident-{e.IncidentId}", new { incidentType = e.IncidentType, estimatedDelayMinutes = e.DelayMinutes }, ct);
        logger.LogInformation("Incident {IncidentId} participant notifications staged", e.IncidentId);
    }
}
