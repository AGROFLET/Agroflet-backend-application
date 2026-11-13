using System.ComponentModel.DataAnnotations;
namespace Agroflet.Platform.Iam.Interfaces.Rest.Resources;

public record SignInResource([Required, EmailAddress] string Email, [Required] string Password);
