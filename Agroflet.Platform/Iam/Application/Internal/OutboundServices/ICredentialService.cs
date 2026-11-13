using Agroflet.Platform.Iam.Domain.Model.Aggregates;
namespace Agroflet.Platform.Iam.Application.Internal.OutboundServices;

public interface ICredentialService { string Hash(User user, string password); bool Verify(User user, string password); string Token(User user); }
