namespace Agroflet.Platform.Fleet.Interfaces.Rest.Resources;

public record UpdateVehicleResource(string? Plate = null, string? Brand = null, string? Model = null, int? Year = null, decimal? CapacityTons = null, string? BodyType = null, string? Status = null);
