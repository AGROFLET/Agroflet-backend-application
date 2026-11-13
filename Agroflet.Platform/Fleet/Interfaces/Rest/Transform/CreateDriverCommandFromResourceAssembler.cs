using Agroflet.Platform.Fleet.Domain.Model.Commands;
using Agroflet.Platform.Fleet.Interfaces.Rest.Resources;
namespace Agroflet.Platform.Fleet.Interfaces.Rest.Transform;

public static class CreateDriverCommandFromResourceAssembler
{
    public static CreateDriverCommand ToCommandFromResource(CreateDriverResource resource, int actorId) => new(actorId, resource.FirstName, resource.LastName, resource.Dni, resource.LicenseNumber, resource.LicenseCategory, resource.Phone, "available", DateTime.UtcNow);
}
