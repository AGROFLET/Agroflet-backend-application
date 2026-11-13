namespace Agroflet.Platform.Shipments.Domain.Model.ValueObjects;

/// <summary>Lifecycle vocabulary shared with the Vue shipment model.</summary>
public sealed record ShipmentStatus
{
    public string Value { get; }
    public ShipmentStatus(string value) => Value = Guard.OneOf(value, "planned", "in_transit", "delivered", "cancelled");
    public bool CanTransitionTo(ShipmentStatus next) =>
        (Value == "planned" && next.Value is "in_transit" or "cancelled") ||
        (Value == "in_transit" && next.Value is "delivered" or "cancelled");
}
