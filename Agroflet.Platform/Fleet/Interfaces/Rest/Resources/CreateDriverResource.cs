using System.ComponentModel.DataAnnotations;
namespace Agroflet.Platform.Fleet.Interfaces.Rest.Resources;

public record CreateDriverResource([Required, StringLength(2000)] string FirstName, [Required, StringLength(2000)] string LastName, [Required, StringLength(2000)] string Dni, [Required, StringLength(2000)] string LicenseNumber, [Required, StringLength(2000)] string LicenseCategory, [Required, StringLength(2000)] string Phone);
