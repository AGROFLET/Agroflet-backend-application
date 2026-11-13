using Agroflet.Platform.Fleet.Domain.Model.Commands;
namespace Agroflet.Platform.Fleet.Domain.Model.Aggregates;

public partial class Driver : Entity
{
    protected Driver() { }
    public int DispatcherId { get; private set; }
    public string FirstName { get; private set; } = "";
    public string LastName { get; private set; } = "";
    public string Dni { get; private set; } = "";
    public string LicenseNumber { get; private set; } = "";
    public string LicenseCategory { get; private set; } = "";
    public string Phone { get; private set; } = "";
    public string Status { get; private set; } = "";
    public DateTime UpdatedAt { get; private set; }
    public Driver(CreateDriverCommand command)
    {
        DispatcherId = command.DispatcherId;
        FirstName = command.FirstName;
        LastName = command.LastName;
        Dni = command.Dni;
        LicenseNumber = command.LicenseNumber;
        LicenseCategory = command.LicenseCategory;
        Phone = command.Phone;
        Status = command.Status;
        UpdatedAt = command.UpdatedAt;
        Validate();
    }
    private void Validate() { FirstName = Guard.Text(FirstName, "first-name"); LastName = Guard.Text(LastName, "last-name"); Guard.Require(System.Text.RegularExpressions.Regex.IsMatch(Dni, @"^\d{8}$"), "dni-format"); LicenseNumber = Guard.Text(LicenseNumber, "license").ToUpperInvariant(); Guard.Require(System.Text.RegularExpressions.Regex.IsMatch(LicenseNumber, @"^[A-Z]\d{8}$"), "license-format"); Guard.OneOf(LicenseCategory, "A-IIa", "A-IIb", "A-IIIa", "A-IIIb", "A-IIIc"); Guard.Require(System.Text.RegularExpressions.Regex.IsMatch(Phone, @"^9\d{8}$"), "phone-format"); Guard.OneOf(Status, "available", "assigned"); }
    public void SetOperationalStatus(string status) { Status = Guard.OneOf(status, "available", "assigned"); UpdatedAt = DateTime.UtcNow; }
    public void Edit(CreateDriverCommand command)
    {
        FirstName = command.FirstName;
        LastName = command.LastName;
        Dni = command.Dni;
        LicenseNumber = command.LicenseNumber;
        LicenseCategory = command.LicenseCategory;
        Phone = command.Phone;
        UpdatedAt = command.UpdatedAt;
        Validate();
    }
}
