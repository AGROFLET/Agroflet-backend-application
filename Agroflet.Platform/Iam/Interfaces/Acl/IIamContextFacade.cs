namespace Agroflet.Platform.Iam.Interfaces.Acl;

public interface IIamContextFacade
{
    Task RequireBuyer(int userId, CancellationToken ct);
}
