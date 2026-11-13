namespace Agroflet.Platform.Incidents.Domain.Model.Commands;

public record CreateIncidentCommand(int ShipmentId, string Type, string Description, int EstimatedDelayMinutes, DateTime OccurredAt, int ReportedBy, string IdempotencyKey, DateTime CreatedAt);
