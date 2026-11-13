using Agroflet.Platform.Iam.Domain.Model.Commands;
using Agroflet.Platform.Iam.Interfaces.Rest.Resources;
namespace Agroflet.Platform.Iam.Interfaces.Rest.Transform;

public static class RequestPasswordResetCommandFromResourceAssembler { public static RequestPasswordResetCommand ToCommandFromResource(RequestPasswordResetResource resource) => new(resource.Email); }
