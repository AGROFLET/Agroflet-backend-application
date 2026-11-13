namespace Agroflet.Platform.Fleet.Interfaces.Rest.Resources;

public record UpdateDriverResource(string? FirstName = null, string? LastName = null, string? Dni = null, string? LicenseNumber = null, string? LicenseCategory = null, string? Phone = null, string? Status = null);
