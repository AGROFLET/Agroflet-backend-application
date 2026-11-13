using Agroflet.Platform.Shipments.Domain.Model.Commands;
using Agroflet.Platform.Shipments.Interfaces.Rest.Resources;
namespace Agroflet.Platform.Shipments.Interfaces.Rest.Transform;

public static class CreateShipmentCommandFromResourceAssembler
{
    public static CreateShipmentCommand ToCommandFromResource(CreateShipmentResource resource, int actorId) => new(Guid.NewGuid().ToString("N"), actorId, resource.BuyerId, resource.CargoType, resource.CargoDescription, resource.WeightTons, resource.OriginId, resource.DestinationId, resource.VehicleId, resource.DriverId, "planned", resource.PlannedDepartureAt, resource.EstimatedArrivalAt, null, null, null, null, DateTime.UtcNow, DateTime.UtcNow);
}
