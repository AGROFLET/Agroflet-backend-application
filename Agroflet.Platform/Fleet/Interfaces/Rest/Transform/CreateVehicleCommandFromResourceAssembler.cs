using Agroflet.Platform.Fleet.Domain.Model.Commands;
using Agroflet.Platform.Fleet.Interfaces.Rest.Resources;
namespace Agroflet.Platform.Fleet.Interfaces.Rest.Transform;

public static class CreateVehicleCommandFromResourceAssembler
{
    public static CreateVehicleCommand ToCommandFromResource(CreateVehicleResource resource, int actorId) => new(actorId, resource.Plate, resource.Brand, resource.Model, resource.Year, resource.CapacityTons, resource.BodyType, "available", DateTime.UtcNow);
}
