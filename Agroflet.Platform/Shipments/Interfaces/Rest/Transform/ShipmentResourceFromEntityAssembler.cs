using Agroflet.Platform.Shipments.Domain.Model.Aggregates;
using Agroflet.Platform.Shipments.Interfaces.Rest.Resources;
namespace Agroflet.Platform.Shipments.Interfaces.Rest.Transform;

public static class ShipmentResourceFromEntityAssembler
{
    public static ShipmentResource ToResourceFromEntity(Shipment entity) => new(entity.Id, entity.Code, entity.DispatcherId, entity.BuyerId, entity.CargoType, entity.CargoDescription, entity.WeightTons, entity.OriginId, entity.DestinationId, entity.VehicleId, entity.DriverId, entity.Status, entity.PlannedDepartureAt, entity.EstimatedArrivalAt, entity.ActualDepartureAt, entity.DeliveredAt, entity.CancelledAt, entity.CancellationReason, entity.CreatedAt, entity.UpdatedAt, entity.Version);
}
