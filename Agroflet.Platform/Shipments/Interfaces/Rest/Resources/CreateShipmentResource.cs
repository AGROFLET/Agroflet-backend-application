using System.ComponentModel.DataAnnotations;
namespace Agroflet.Platform.Shipments.Interfaces.Rest.Resources;

public record CreateShipmentResource(int BuyerId, [Required, StringLength(2000)] string CargoType, [Required(AllowEmptyStrings = true), StringLength(2000)] string CargoDescription, decimal WeightTons, int OriginId, int DestinationId, int VehicleId, int DriverId, DateTime PlannedDepartureAt, DateTime EstimatedArrivalAt);
