using Agroflet.Platform.Shipments.Domain.Model.Commands;
namespace Agroflet.Platform.Shipments.Domain.Model.Aggregates;

public partial class StatusChange : Entity
{
    protected StatusChange() { }
    public int ShipmentId { get; private set; }
    public string? PreviousStatus { get; private set; }
    public string NewStatus { get; private set; } = "";
    public DateTime ChangedAt { get; private set; }
    public int ChangedBy { get; private set; }
    public string? Reason { get; private set; }
    public StatusChange(CreateStatusChangeCommand command)
    {
        ShipmentId = command.ShipmentId;
        PreviousStatus = command.PreviousStatus;
        NewStatus = command.NewStatus;
        ChangedAt = command.ChangedAt;
        ChangedBy = command.ChangedBy;
        Reason = command.Reason;
        Validate();
    }
    private void Validate() { }


}
