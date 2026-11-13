namespace Agroflet.Platform.Iam.Domain.Model.Aggregates;

public class RevokedSession
{
    public string Jti { get; private set; } = "";
    public int UserId { get; private set; }
    public DateTime ExpiresAt { get; private set; }
    protected RevokedSession() { }
    public RevokedSession(string jti, int userId, DateTime expiresAt) { Jti = jti; UserId = userId; ExpiresAt = expiresAt; }
}
