using Agroflet.Platform.Incidents.Domain.Model.Commands;
namespace Agroflet.Platform.Incidents.Domain.Model.Aggregates;

public partial class Incident : Agroflet.Platform.Shared.Domain.Model.Aggregates.Internal.AggregateRoot
{
    protected Incident() { }
    public int ShipmentId { get; private set; }
    public string Type { get; private set; } = "";
    public string Description { get; private set; } = "";
    public int EstimatedDelayMinutes { get; private set; }
    public DateTime OccurredAt { get; private set; }
    public int ReportedBy { get; private set; }
    public string IdempotencyKey { get; private set; } = "";
    public DateTime CreatedAt { get; private set; }
    public Incident(CreateIncidentCommand command)
    {
        ShipmentId = command.ShipmentId;
        Type = command.Type;
        Description = command.Description;
        EstimatedDelayMinutes = command.EstimatedDelayMinutes;
        OccurredAt = command.OccurredAt;
        ReportedBy = command.ReportedBy;
        IdempotencyKey = command.IdempotencyKey;
        CreatedAt = command.CreatedAt;
        Validate();
    }
    public void RecordReported() => AddDomainEvent(new Agroflet.Platform.Incidents.Domain.Model.Events.IncidentReportedEvent(
        new(Id), new(ShipmentId), new(ReportedBy), Type, EstimatedDelayMinutes));
    private void Validate() { Guard.OneOf(Type, "road_block", "landslide", "mechanical_failure", "accident", "heavy_traffic", "weather", "other"); Description = Guard.Text(Description, "description", 2000); IdempotencyKey = Guard.Text(IdempotencyKey, "idempotency-key", 100); Guard.Require(EstimatedDelayMinutes >= 0 && EstimatedDelayMinutes <= 10080, "delay-range"); Guard.Require(OccurredAt != default && OccurredAt <= DateTime.UtcNow.AddMinutes(1), "occurred-at-future"); }


}
