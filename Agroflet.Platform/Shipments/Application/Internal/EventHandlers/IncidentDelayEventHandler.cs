using Agroflet.Platform.Shared.Application.Internal.EventHandlers;
using Agroflet.Platform.Incidents.Domain.Model.Events;
using Agroflet.Platform.Shipments.Domain.Repositories;
namespace Agroflet.Platform.Shipments.Application.Internal.EventHandlers;

public class IncidentDelayEventHandler(IShipmentRepository shipments, ILogger<IncidentDelayEventHandler> logger) : IEventHandler<IncidentReportedEvent>
{
    public async Task Handle(IncidentReportedEvent e, CancellationToken ct)
    {
        var shipment = await shipments.FindByIdAsync(e.ShipmentId.Value, ct) ?? throw new BusinessException("not-found", 404);
        if (shipment.DispatcherId != e.DispatcherId.Value) throw new BusinessException("not-found", 404);
        shipment.ApplyIncident(e.IncidentId.Value, e.Type, e.DelayMinutes);
        logger.LogInformation("Incident {IncidentId} ETA change staged for shipment {ShipmentId}", e.IncidentId, e.ShipmentId);
    }
}
