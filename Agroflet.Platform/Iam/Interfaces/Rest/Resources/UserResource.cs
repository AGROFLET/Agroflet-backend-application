namespace Agroflet.Platform.Iam.Interfaces.Rest.Resources;

public record UserResource(int Id, string FirstName, string LastName, string Email, string UserType, string Phone, string CompanyName, string PreferredLanguage);
