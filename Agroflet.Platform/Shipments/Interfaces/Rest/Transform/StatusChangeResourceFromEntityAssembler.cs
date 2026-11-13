using Agroflet.Platform.Shipments.Domain.Model.Aggregates;
using Agroflet.Platform.Shipments.Interfaces.Rest.Resources;
namespace Agroflet.Platform.Shipments.Interfaces.Rest.Transform;

public static class StatusChangeResourceFromEntityAssembler
{
    public static StatusChangeResource ToResourceFromEntity(StatusChange entity) => new(entity.Id, entity.ShipmentId, entity.PreviousStatus, entity.NewStatus, entity.ChangedAt, entity.ChangedBy, entity.Reason);
}
