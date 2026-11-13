using Agroflet.Platform.Fleet.Domain.Model.Commands;
using Agroflet.Platform.Fleet.Interfaces.Rest.Resources;
namespace Agroflet.Platform.Fleet.Interfaces.Rest.Transform;

public static class UpdateVehicleCommandFromResourceAssembler
{
    public static UpdateVehicleCommand ToCommandFromResource(int id, UpdateVehicleResource resource) => new(id, resource.Plate, resource.Brand, resource.Model, resource.Year, resource.CapacityTons, resource.BodyType, resource.Status);
}
