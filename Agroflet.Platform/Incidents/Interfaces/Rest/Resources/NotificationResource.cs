namespace Agroflet.Platform.Incidents.Interfaces.Rest.Resources;

public record NotificationResource(int Id, int RecipientId, int ShipmentId, string ShipmentCode, string Type, string EventKey, System.Text.Json.JsonElement Payload, DateTime CreatedAt, bool IsRead, DateTime? ReadAt);
