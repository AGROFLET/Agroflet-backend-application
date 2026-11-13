using Agroflet.Platform.Iam.Domain.Model.Aggregates;
using Agroflet.Platform.Iam.Interfaces.Rest.Resources;
namespace Agroflet.Platform.Iam.Interfaces.Rest.Transform;

public static class UserResourceFromEntityAssembler
{
    public static UserResource ToResourceFromEntity(User entity) => new(entity.Id, entity.FirstName, entity.LastName, entity.Email, entity.UserType, entity.Phone, entity.CompanyName, entity.PreferredLanguage);
}
