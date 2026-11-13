namespace Agroflet.Platform.Incidents.Domain.Model.Commands;

public record CreateNotificationCommand(int RecipientId, int ShipmentId, string ShipmentCode, string Type, string EventKey, string PayloadJson, DateTime CreatedAt, bool IsRead, DateTime? ReadAt);
