using Agroflet.Platform.Shipments.Domain.Model.Commands;
using Agroflet.Platform.Shipments.Interfaces.Rest.Resources;
namespace Agroflet.Platform.Shipments.Interfaces.Rest.Transform;

public static class UpdateShipmentCommandFromResourceAssembler
{
    public static UpdateShipmentCommand ToCommandFromResource(int id, UpdateShipmentResource resource) => new(id, resource.Status, resource.CancellationReason, resource.ExpectedVersion);
}
