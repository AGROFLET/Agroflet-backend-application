using Agroflet.Platform.Incidents.Domain.Model.Aggregates;
using Agroflet.Platform.Incidents.Interfaces.Rest.Resources;
namespace Agroflet.Platform.Incidents.Interfaces.Rest.Transform;

public static class NotificationResourceFromEntityAssembler
{
    public static NotificationResource ToResourceFromEntity(Notification entity) => new(entity.Id, entity.RecipientId, entity.ShipmentId, entity.ShipmentCode, entity.Type, entity.EventKey, System.Text.Json.JsonSerializer.Deserialize<System.Text.Json.JsonElement>(entity.PayloadJson), entity.CreatedAt, entity.IsRead, entity.ReadAt);
}
