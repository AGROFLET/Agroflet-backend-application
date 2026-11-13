namespace Agroflet.Platform.Shipments.Domain.Model.Commands;

public record UpdateShipmentCommand(int Id, string? Status, string? CancellationReason, int? ExpectedVersion = null);
