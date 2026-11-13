using Agroflet.Platform.Iam.Domain.Model.Aggregates;
namespace Agroflet.Platform.Iam.Application.Internal.OutboundServices;

public interface ITokenService { string Token(User user); }
