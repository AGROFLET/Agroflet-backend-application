namespace Agroflet.Platform.Incidents.Interfaces.Rest.Resources;

public record IncidentResource(int Id, int ShipmentId, string Type, string Description, int EstimatedDelayMinutes, DateTime OccurredAt, int ReportedBy, string IdempotencyKey, DateTime CreatedAt);
