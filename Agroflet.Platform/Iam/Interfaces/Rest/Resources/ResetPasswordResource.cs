using System.ComponentModel.DataAnnotations;
namespace Agroflet.Platform.Iam.Interfaces.Rest.Resources;

public record ResetPasswordResource([Required] string Token, [Required, StringLength(128, MinimumLength = 8)] string NewPassword);
