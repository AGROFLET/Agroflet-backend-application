using Agroflet.Platform.Shipments.Domain.Model.Aggregates;
using Agroflet.Platform.Shipments.Interfaces.Rest.Resources;
namespace Agroflet.Platform.Shipments.Interfaces.Rest.Transform;

public static class LocationResourceFromEntityAssembler
{
    public static LocationResource ToResourceFromEntity(Location entity) => new(entity.Id, entity.Name, entity.City, entity.Region, entity.Latitude, entity.Longitude, entity.Kind);
}
