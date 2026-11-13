namespace Agroflet.Platform.Fleet.Interfaces.Rest.Resources;

public record VehicleResource(int Id, int DispatcherId, string Plate, string Brand, string Model, int Year, decimal CapacityTons, string BodyType, string Status, DateTime UpdatedAt);
