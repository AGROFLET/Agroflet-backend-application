using System.ComponentModel.DataAnnotations;
namespace Agroflet.Platform.Fleet.Interfaces.Rest.Resources;

public record CreateVehicleResource([Required, StringLength(2000)] string Plate, [Required, StringLength(2000)] string Brand, [Required, StringLength(2000)] string Model, int Year, decimal CapacityTons, [Required, StringLength(2000)] string BodyType);
