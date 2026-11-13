using Agroflet.Platform.Iam.Domain.Model.Commands;
namespace Agroflet.Platform.Iam.Domain.Model.Aggregates;

public partial class User : Entity
{
    protected User() { }
    public string FirstName { get; private set; } = "";
    public string LastName { get; private set; } = "";
    public string Email { get; private set; } = "";
    public string UserType { get; private set; } = "";
    public string Phone { get; private set; } = "";
    public string CompanyName { get; private set; } = "";
    public string PreferredLanguage { get; private set; } = "";
    public User(CreateUserCommand command)
    {
        FirstName = command.FirstName;
        LastName = command.LastName;
        Email = command.Email;
        UserType = command.UserType;
        Phone = command.Phone;
        CompanyName = command.CompanyName;
        PreferredLanguage = command.PreferredLanguage;
        Validate();
    }
    private void Validate() { FirstName = Guard.Text(FirstName, "first-name"); LastName = Guard.Text(LastName, "last-name"); Email = Guard.Text(Email, "email").ToLowerInvariant(); Guard.Require(new System.ComponentModel.DataAnnotations.EmailAddressAttribute().IsValid(Email), "email-invalid"); Guard.OneOf(UserType, "dispatcher", "buyer"); PreferredLanguage = PreferredLanguage switch { "es_419" => "es", "en_US" => "en", _ => PreferredLanguage }; Guard.OneOf(PreferredLanguage, "es", "en"); Guard.Require(Phone == "" || System.Text.RegularExpressions.Regex.IsMatch(Phone, @"^9\d{8}$"), "phone-format"); }
    public string PasswordHash { get; private set; } = "";
    public int TokenVersion { get; private set; }
    public void SetPassword(string hash) { PasswordHash = hash; TokenVersion++; }
    public void Edit(CreateUserCommand command)
    {
        FirstName = command.FirstName;
        LastName = command.LastName;
        Phone = command.Phone;
        CompanyName = command.CompanyName;
        PreferredLanguage = command.PreferredLanguage;
        Validate();
    }
}
