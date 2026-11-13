using Agroflet.Platform.Tracking.Domain.Model.Commands;
namespace Agroflet.Platform.Tracking.Domain.Model.Aggregates;

public partial class ReportedPosition : Entity
{
    protected ReportedPosition() { }
    public string? EventId { get; private set; }
    public int ShipmentId { get; private set; }
    public double Latitude { get; private set; }
    public double Longitude { get; private set; }
    public DateTime ReportedAt { get; private set; }
    public DateTime RecordedAt { get; private set; }
    public string Source { get; private set; } = "";
    public int RecordedBy { get; private set; }
    public bool IsDemo { get; private set; }
    public string Note { get; private set; } = "";
    public ReportedPosition(CreateReportedPositionCommand command)
    {
        EventId = command.EventId == null ? null : Guard.Text(command.EventId, "event-id", 128);
        ShipmentId = command.ShipmentId;
        Latitude = command.Latitude;
        Longitude = command.Longitude;
        ReportedAt = command.ReportedAt;
        RecordedAt = command.RecordedAt;
        Source = command.Source;
        RecordedBy = command.RecordedBy;
        IsDemo = command.IsDemo;
        Note = command.Note;
        Validate();
    }
    private void Validate()
    {
        _ = new ValueObjects.Coordinates(Latitude, Longitude);
        Guard.Require(ReportedAt != default && ReportedAt <= DateTime.UtcNow.AddMinutes(1), "reported-at-future");
        Guard.OneOf(Source, "driver_call", "whatsapp_message", "gps_device", "checkpoint");
    }


}
