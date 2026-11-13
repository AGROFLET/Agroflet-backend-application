namespace Agroflet.Platform.Fleet.Interfaces.Rest.Resources;

public record DriverResource(int Id, int DispatcherId, string FirstName, string LastName, string Dni, string LicenseNumber, string LicenseCategory, string Phone, string Status, DateTime UpdatedAt);
