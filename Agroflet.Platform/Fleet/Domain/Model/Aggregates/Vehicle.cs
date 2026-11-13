using Agroflet.Platform.Fleet.Domain.Model.Commands;
namespace Agroflet.Platform.Fleet.Domain.Model.Aggregates;

public partial class Vehicle : Entity
{
    protected Vehicle() { }
    public int DispatcherId { get; private set; }
    public string Plate { get; private set; } = "";
    public string Brand { get; private set; } = "";
    public string Model { get; private set; } = "";
    public int Year { get; private set; }
    public decimal CapacityTons { get; private set; }
    public string BodyType { get; private set; } = "";
    public string Status { get; private set; } = "";
    public DateTime UpdatedAt { get; private set; }
    public Vehicle(CreateVehicleCommand command)
    {
        DispatcherId = command.DispatcherId;
        Plate = command.Plate;
        Brand = command.Brand;
        Model = command.Model;
        Year = command.Year;
        CapacityTons = command.CapacityTons;
        BodyType = command.BodyType;
        Status = command.Status;
        UpdatedAt = command.UpdatedAt;
        Validate();
    }
    private void Validate() { Plate = Guard.Text(Plate, "plate").ToUpperInvariant(); Guard.Require(System.Text.RegularExpressions.Regex.IsMatch(Plate, @"^[A-Z0-9]{3}-\d{3}$"), "plate-format"); Brand = Guard.Text(Brand, "brand"); Model = Guard.Text(Model, "model"); Guard.Require(Year >= 1980 && Year <= DateTime.UtcNow.Year + 1, "year-range"); Guard.Require(CapacityTons > 0, "capacity-positive"); Guard.OneOf(BodyType, "refrigerated_van", "dry_van", "stake_bed", "flatbed", "tanker", "other"); Guard.OneOf(Status, "available", "reserved", "in_route", "under_maintenance"); }
    public void SetOperationalStatus(string status) { Status = Guard.OneOf(status, "available", "reserved", "in_route", "under_maintenance"); UpdatedAt = DateTime.UtcNow; }
    public void Edit(CreateVehicleCommand command)
    {
        Plate = command.Plate;
        Brand = command.Brand;
        Model = command.Model;
        Year = command.Year;
        CapacityTons = command.CapacityTons;
        BodyType = command.BodyType;
        UpdatedAt = command.UpdatedAt;
        Validate();
    }
}
