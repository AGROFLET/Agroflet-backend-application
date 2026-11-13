namespace Agroflet.Platform.Iam.Application.Internal.OutboundServices;

public interface ISessionRevocationService
{
    Task Revoke(string jti, int userId, DateTime expiresAt, CancellationToken ct);
    Task<bool> IsRevoked(string jti, CancellationToken ct);
}
