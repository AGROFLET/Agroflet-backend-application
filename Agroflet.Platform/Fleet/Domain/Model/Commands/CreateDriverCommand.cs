namespace Agroflet.Platform.Fleet.Domain.Model.Commands;

public record CreateDriverCommand(int DispatcherId, string FirstName, string LastName, string Dni, string LicenseNumber, string LicenseCategory, string Phone, string Status, DateTime UpdatedAt);
