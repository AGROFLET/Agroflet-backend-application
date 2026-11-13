namespace Agroflet.Platform.Iam.Domain.Model.Commands;

public record SignUpCommand(string FirstName, string LastName, string Email, string Password, string UserType, string Phone = "", string CompanyName = "", string PreferredLanguage = "en");
