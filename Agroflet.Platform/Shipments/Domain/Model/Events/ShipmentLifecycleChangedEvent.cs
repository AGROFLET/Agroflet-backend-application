using Agroflet.Platform.Shared.Domain.Model.Events;
using Agroflet.Platform.Shared.Domain.Model.Identifiers;
namespace Agroflet.Platform.Shipments.Domain.Model.Events;

public record ShipmentLifecycleChangedEvent(ShipmentId ShipmentId, string Code, UserId DispatcherId, UserId BuyerId,
    VehicleId VehicleId, DriverId DriverId, string? PreviousStatus, string Status, DateTime ChangedAt, string? Reason) : IEvent;
