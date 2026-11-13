using System.ComponentModel.DataAnnotations;
namespace Agroflet.Platform.Iam.Interfaces.Rest.Resources;

public record RequestPasswordResetResource([Required, EmailAddress] string Email);
