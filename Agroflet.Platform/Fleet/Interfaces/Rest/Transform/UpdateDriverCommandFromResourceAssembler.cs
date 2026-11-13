using Agroflet.Platform.Fleet.Domain.Model.Commands;
using Agroflet.Platform.Fleet.Interfaces.Rest.Resources;
namespace Agroflet.Platform.Fleet.Interfaces.Rest.Transform;

public static class UpdateDriverCommandFromResourceAssembler
{
    public static UpdateDriverCommand ToCommandFromResource(int id, UpdateDriverResource resource) => new(id, resource.FirstName, resource.LastName, resource.Dni, resource.LicenseNumber, resource.LicenseCategory, resource.Phone, resource.Status);
}
