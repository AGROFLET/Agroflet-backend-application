using Agroflet.Platform.Iam.Domain.Model.Commands;
using Agroflet.Platform.Iam.Interfaces.Rest.Resources;
namespace Agroflet.Platform.Iam.Interfaces.Rest.Transform;

public static class ResetPasswordCommandFromResourceAssembler { public static ResetPasswordCommand ToCommandFromResource(ResetPasswordResource resource) => new(resource.Token, resource.NewPassword); }
