using Agroflet.Platform.Fleet.Domain.Model.Aggregates;
using Agroflet.Platform.Fleet.Interfaces.Rest.Resources;
namespace Agroflet.Platform.Fleet.Interfaces.Rest.Transform;

public static class VehicleResourceFromEntityAssembler
{
    public static VehicleResource ToResourceFromEntity(Vehicle entity) => new(entity.Id, entity.DispatcherId, entity.Plate, entity.Brand, entity.Model, entity.Year, entity.CapacityTons, entity.BodyType, entity.Status, entity.UpdatedAt);
}
