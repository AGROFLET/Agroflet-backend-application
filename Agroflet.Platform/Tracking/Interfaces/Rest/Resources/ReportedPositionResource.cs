namespace Agroflet.Platform.Tracking.Interfaces.Rest.Resources;

public record ReportedPositionResource(int Id, int ShipmentId, double Latitude, double Longitude, DateTime ReportedAt, DateTime RecordedAt, string Source, int RecordedBy, bool IsDemo, string Note, string? EventId);
