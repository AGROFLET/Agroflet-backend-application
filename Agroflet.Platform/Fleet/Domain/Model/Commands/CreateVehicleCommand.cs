namespace Agroflet.Platform.Fleet.Domain.Model.Commands;

public record CreateVehicleCommand(int DispatcherId, string Plate, string Brand, string Model, int Year, decimal CapacityTons, string BodyType, string Status, DateTime UpdatedAt);
