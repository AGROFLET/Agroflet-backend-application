using System.ComponentModel.DataAnnotations;
namespace Agroflet.Platform.Iam.Interfaces.Rest.Resources;

public record ChangePasswordResource([Required] string CurrentPassword, [Required, StringLength(128, MinimumLength = 8)] string NewPassword);
