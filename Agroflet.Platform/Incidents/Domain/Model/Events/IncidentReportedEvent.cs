using Agroflet.Platform.Shared.Domain.Model.Events;
using Agroflet.Platform.Shared.Domain.Model.Identifiers;
namespace Agroflet.Platform.Incidents.Domain.Model.Events;

public record IncidentReportedEvent(IncidentId IncidentId, ShipmentId ShipmentId, UserId DispatcherId, string Type, int DelayMinutes) : IEvent;
