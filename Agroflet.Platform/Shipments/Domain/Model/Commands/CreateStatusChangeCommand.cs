namespace Agroflet.Platform.Shipments.Domain.Model.Commands;

public record CreateStatusChangeCommand(int ShipmentId, string? PreviousStatus, string NewStatus, DateTime ChangedAt, int ChangedBy, string? Reason);
