namespace Agroflet.Platform.Shipments.Domain.Model.Commands;

public record CreateShipmentCommand(string Code, int DispatcherId, int BuyerId, string CargoType, string CargoDescription, decimal WeightTons, int OriginId, int DestinationId, int VehicleId, int DriverId, string Status, DateTime PlannedDepartureAt, DateTime EstimatedArrivalAt, DateTime? ActualDepartureAt, DateTime? DeliveredAt, DateTime? CancelledAt, string? CancellationReason, DateTime CreatedAt, DateTime UpdatedAt);
