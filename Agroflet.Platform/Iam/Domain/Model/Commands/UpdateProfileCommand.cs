namespace Agroflet.Platform.Iam.Domain.Model.Commands;

public record UpdateProfileCommand(string? FirstName = null, string? LastName = null, string? Phone = null, string? CompanyName = null, string? PreferredLanguage = null);
