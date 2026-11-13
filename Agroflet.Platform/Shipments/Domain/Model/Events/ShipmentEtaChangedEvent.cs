using Agroflet.Platform.Shared.Domain.Model.Events;
using Agroflet.Platform.Shared.Domain.Model.Identifiers;
namespace Agroflet.Platform.Shipments.Domain.Model.Events;

public record ShipmentEtaChangedEvent(ShipmentId ShipmentId, string Code, UserId DispatcherId, UserId BuyerId, IncidentId IncidentId, string IncidentType, int DelayMinutes) : IEvent;
