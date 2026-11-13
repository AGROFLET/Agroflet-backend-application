namespace Agroflet.Platform.Fleet.Domain.Model.Commands;

public record UpdateVehicleCommand(int Id, string? Plate, string? Brand, string? Model, int? Year, decimal? CapacityTons, string? BodyType, string? Status);
