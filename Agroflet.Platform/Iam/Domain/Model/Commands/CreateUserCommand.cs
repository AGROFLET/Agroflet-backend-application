namespace Agroflet.Platform.Iam.Domain.Model.Commands;

public record CreateUserCommand(string FirstName, string LastName, string Email, string UserType, string Phone, string CompanyName, string PreferredLanguage);
