using Agroflet.Platform.Iam.Domain.Model.Commands;
using Agroflet.Platform.Iam.Interfaces.Rest.Resources;
namespace Agroflet.Platform.Iam.Interfaces.Rest.Transform;

public static class SignUpCommandFromResourceAssembler { public static SignUpCommand ToCommandFromResource(SignUpResource resource) => new(resource.FirstName, resource.LastName, resource.Email, resource.Password, resource.UserType, resource.Phone, resource.CompanyName, resource.PreferredLanguage); }
