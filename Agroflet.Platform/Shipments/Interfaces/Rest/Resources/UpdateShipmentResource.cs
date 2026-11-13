namespace Agroflet.Platform.Shipments.Interfaces.Rest.Resources;

public record UpdateShipmentResource(string? Status = null, string? CancellationReason = null, int? ExpectedVersion = null);
