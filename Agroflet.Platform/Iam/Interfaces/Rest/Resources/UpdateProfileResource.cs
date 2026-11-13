using System.ComponentModel.DataAnnotations;
namespace Agroflet.Platform.Iam.Interfaces.Rest.Resources;

public record UpdateProfileResource(string? FirstName = null, string? LastName = null, string? Phone = null, string? CompanyName = null, string? PreferredLanguage = null);
