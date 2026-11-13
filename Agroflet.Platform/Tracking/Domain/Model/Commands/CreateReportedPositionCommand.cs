namespace Agroflet.Platform.Tracking.Domain.Model.Commands;

public record CreateReportedPositionCommand(int ShipmentId, double Latitude, double Longitude, DateTime ReportedAt, DateTime RecordedAt, string Source, int RecordedBy, bool IsDemo, string Note, string? EventId = null);
