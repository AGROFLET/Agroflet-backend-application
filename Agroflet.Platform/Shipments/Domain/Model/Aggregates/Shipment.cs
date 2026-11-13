using Agroflet.Platform.Shipments.Domain.Model.Commands;
namespace Agroflet.Platform.Shipments.Domain.Model.Aggregates;

public partial class Shipment : Agroflet.Platform.Shared.Domain.Model.Aggregates.Internal.AggregateRoot
{
    protected Shipment() { }
    public string Code { get; private set; } = "";
    public int DispatcherId { get; private set; }
    public int BuyerId { get; private set; }
    public string CargoType { get; private set; } = "";
    public string CargoDescription { get; private set; } = "";
    public decimal WeightTons { get; private set; }
    public int OriginId { get; private set; }
    public int DestinationId { get; private set; }
    public int VehicleId { get; private set; }
    public int DriverId { get; private set; }
    public string Status { get; private set; } = "";
    public DateTime PlannedDepartureAt { get; private set; }
    public DateTime EstimatedArrivalAt { get; private set; }
    public DateTime? ActualDepartureAt { get; private set; }
    public DateTime? DeliveredAt { get; private set; }
    public DateTime? CancelledAt { get; private set; }
    public string? CancellationReason { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
    public Shipment(CreateShipmentCommand command)
    {
        Code = command.Code;
        DispatcherId = command.DispatcherId;
        BuyerId = command.BuyerId;
        CargoType = command.CargoType;
        CargoDescription = command.CargoDescription;
        WeightTons = command.WeightTons;
        OriginId = command.OriginId;
        DestinationId = command.DestinationId;
        VehicleId = command.VehicleId;
        DriverId = command.DriverId;
        Status = command.Status;
        PlannedDepartureAt = command.PlannedDepartureAt;
        EstimatedArrivalAt = command.EstimatedArrivalAt;
        ActualDepartureAt = command.ActualDepartureAt;
        DeliveredAt = command.DeliveredAt;
        CancelledAt = command.CancelledAt;
        CancellationReason = command.CancellationReason;
        CreatedAt = command.CreatedAt;
        UpdatedAt = command.UpdatedAt;
        Validate();
    }
    private void Validate() { Guard.Require(BuyerId > 0 && VehicleId > 0 && DriverId > 0, "references-required"); Guard.Require(OriginId > 0 && DestinationId > 0 && OriginId != DestinationId, "same-origin-destination"); Guard.Require(WeightTons > 0, "weight-positive"); Guard.Require(PlannedDepartureAt != default && EstimatedArrivalAt > PlannedDepartureAt, "arrival-before-departure"); Guard.OneOf(CargoType, "potato", "carrot", "onion", "fresh_fruit", "leafy_vegetables", "grains", "other"); Guard.OneOf(Status, "planned", "in_transit", "delivered", "cancelled"); }
    public void AssignCode()
    {
        Guard.Require(Id > 0 && !Code.StartsWith("AGF-"), "shipment-code-already-assigned");
        Code = $"AGF-{Id:D4}";
        RecordLifecycle(null);
    }
    private void RecordLifecycle(string? previous) => AddDomainEvent(
        new Agroflet.Platform.Shipments.Domain.Model.Events.ShipmentLifecycleChangedEvent(
            new(Id), Code, new(DispatcherId), new(BuyerId), new(VehicleId), new(DriverId), previous, Status, UpdatedAt, CancellationReason));
    public void Touch() => UpdatedAt = DateTime.UtcNow;
    public void Delay(int minutes) { Guard.Require(Status == "in_transit", "not-in-transit"); EstimatedArrivalAt = EstimatedArrivalAt.AddMinutes(minutes); Touch(); }
    public void ApplyIncident(int incidentId, string type, int minutes)
    {
        Delay(minutes);
        AddDomainEvent(new Agroflet.Platform.Shipments.Domain.Model.Events.ShipmentEtaChangedEvent(new(Id), Code, new(DispatcherId), new(BuyerId), new(incidentId), type, minutes));
    }
    public void Transition(string next, string? reason)
    {
        if (!new ValueObjects.ShipmentStatus(Status).CanTransitionTo(new ValueObjects.ShipmentStatus(next))) throw new BusinessException("invalid-transition", 409);
        var previous = Status;
        var now = DateTime.UtcNow;
        if (next == "cancelled") { CancellationReason = Guard.Text(reason, "reason", 2000); CancelledAt = now; }
        if (next == "in_transit") ActualDepartureAt = now;
        if (next == "delivered") DeliveredAt = now;
        Status = next; UpdatedAt = now;
        RecordLifecycle(previous);
    }

}
