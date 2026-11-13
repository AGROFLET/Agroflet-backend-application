using System.ComponentModel.DataAnnotations;
namespace Agroflet.Platform.Iam.Interfaces.Rest.Resources;

public record SignUpResource([Required] string FirstName, [Required] string LastName, [Required, EmailAddress] string Email, [Required, StringLength(128, MinimumLength = 8)] string Password, [Required] string UserType, string Phone = "", string CompanyName = "", string PreferredLanguage = "en");
