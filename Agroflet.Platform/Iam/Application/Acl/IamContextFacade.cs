using Agroflet.Platform.Iam.Domain.Repositories;
using Agroflet.Platform.Iam.Interfaces.Acl;
namespace Agroflet.Platform.Iam.Application.Acl;

public class IamContextFacade(IUserRepository users) : IIamContextFacade
{
    public async Task RequireBuyer(int userId, CancellationToken ct)
    {
        var buyer = await users.FindByIdAsync(userId, ct);
        Guard.Require(buyer?.UserType == "buyer", "buyer-required");
    }
}
