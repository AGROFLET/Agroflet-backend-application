using Agroflet.Platform.Fleet.Domain.Model.Aggregates;
using Agroflet.Platform.Fleet.Interfaces.Rest.Resources;
namespace Agroflet.Platform.Fleet.Interfaces.Rest.Transform;

public static class DriverResourceFromEntityAssembler
{
    public static DriverResource ToResourceFromEntity(Driver entity, bool includePrivate = true) => new(entity.Id, entity.DispatcherId, entity.FirstName, entity.LastName, includePrivate ? entity.Dni : "", includePrivate ? entity.LicenseNumber : "", includePrivate ? entity.LicenseCategory : "", includePrivate ? entity.Phone : "", entity.Status, entity.UpdatedAt);
}
