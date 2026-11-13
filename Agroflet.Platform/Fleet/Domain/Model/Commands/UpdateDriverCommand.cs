namespace Agroflet.Platform.Fleet.Domain.Model.Commands;

public record UpdateDriverCommand(int Id, string? FirstName, string? LastName, string? Dni, string? LicenseNumber, string? LicenseCategory, string? Phone, string? Status);
