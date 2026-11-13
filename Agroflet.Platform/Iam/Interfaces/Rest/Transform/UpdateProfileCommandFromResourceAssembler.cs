using Agroflet.Platform.Iam.Domain.Model.Commands;
using Agroflet.Platform.Iam.Interfaces.Rest.Resources;
namespace Agroflet.Platform.Iam.Interfaces.Rest.Transform;

public static class UpdateProfileCommandFromResourceAssembler { public static UpdateProfileCommand ToCommandFromResource(UpdateProfileResource resource) => new(resource.FirstName, resource.LastName, resource.Phone, resource.CompanyName, resource.PreferredLanguage); }
