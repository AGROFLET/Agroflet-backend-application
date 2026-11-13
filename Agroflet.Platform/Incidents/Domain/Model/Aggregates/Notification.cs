using Agroflet.Platform.Incidents.Domain.Model.Commands;
namespace Agroflet.Platform.Incidents.Domain.Model.Aggregates;

public partial class Notification : Entity
{
    protected Notification() { }
    public int RecipientId { get; private set; }
    public int ShipmentId { get; private set; }
    public string ShipmentCode { get; private set; } = "";
    public string Type { get; private set; } = "";
    public string EventKey { get; private set; } = "";
    public string PayloadJson { get; private set; } = "";
    public DateTime CreatedAt { get; private set; }
    public bool IsRead { get; private set; }
    public DateTime? ReadAt { get; private set; }
    public Notification(CreateNotificationCommand command)
    {
        RecipientId = command.RecipientId;
        ShipmentId = command.ShipmentId;
        ShipmentCode = command.ShipmentCode;
        Type = command.Type;
        EventKey = command.EventKey;
        PayloadJson = command.PayloadJson;
        CreatedAt = command.CreatedAt;
        IsRead = command.IsRead;
        ReadAt = command.ReadAt;
        Validate();
    }
    private void Validate() { }
    public void MarkRead() { if (!IsRead) { IsRead = true; ReadAt = DateTime.UtcNow; } }

}
