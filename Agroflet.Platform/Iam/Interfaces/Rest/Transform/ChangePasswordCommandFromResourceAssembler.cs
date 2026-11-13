using Agroflet.Platform.Iam.Domain.Model.Commands;
using Agroflet.Platform.Iam.Interfaces.Rest.Resources;
namespace Agroflet.Platform.Iam.Interfaces.Rest.Transform;

public static class ChangePasswordCommandFromResourceAssembler { public static ChangePasswordCommand ToCommandFromResource(ChangePasswordResource resource) => new(resource.CurrentPassword, resource.NewPassword); }
