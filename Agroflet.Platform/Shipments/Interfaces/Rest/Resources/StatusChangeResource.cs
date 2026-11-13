namespace Agroflet.Platform.Shipments.Interfaces.Rest.Resources;

public record StatusChangeResource(int Id, int ShipmentId, string? PreviousStatus, string NewStatus, DateTime ChangedAt, int ChangedBy, string? Reason);
