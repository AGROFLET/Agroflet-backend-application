using Agroflet.Platform.Iam.Domain.Model.Commands;
using Agroflet.Platform.Iam.Interfaces.Rest.Resources;
namespace Agroflet.Platform.Iam.Interfaces.Rest.Transform;

public static class SignInCommandFromResourceAssembler { public static SignInCommand ToCommandFromResource(SignInResource resource) => new(resource.Email, resource.Password); }
