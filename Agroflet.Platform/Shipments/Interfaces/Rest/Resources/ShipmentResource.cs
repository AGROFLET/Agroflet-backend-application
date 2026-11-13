namespace Agroflet.Platform.Shipments.Interfaces.Rest.Resources;

public record ShipmentResource(int Id, string Code, int DispatcherId, int BuyerId, string CargoType, string CargoDescription, decimal WeightTons, int OriginId, int DestinationId, int VehicleId, int DriverId, string Status, DateTime PlannedDepartureAt, DateTime EstimatedArrivalAt, DateTime? ActualDepartureAt, DateTime? DeliveredAt, DateTime? CancelledAt, string? CancellationReason, DateTime CreatedAt, DateTime UpdatedAt, int Version);
