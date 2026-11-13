using Agroflet.Platform.Iam.Domain.Model.Aggregates;
using Agroflet.Platform.Iam.Application.Internal.OutboundServices;
namespace Agroflet.Platform.Iam.Infrastructure.Authentication;
/// <summary>Compatibility facade composing the hashing and token outbound services.</summary>
public sealed class CredentialService(IHashingService hashing, ITokenService tokens) : ICredentialService
{
    public string Hash(User user, string password) => hashing.Hash(user, password);
    public bool Verify(User user, string password) => hashing.Verify(user, password);
    public string Token(User user) => tokens.Token(user);
}
