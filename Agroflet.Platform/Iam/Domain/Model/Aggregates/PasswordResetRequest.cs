namespace Agroflet.Platform.Iam.Domain.Model.Aggregates;

public class PasswordResetRequest : Entity
{
    protected PasswordResetRequest() { }
    public int UserId { get; private set; }
    public string TokenHash { get; private set; } = "";
    public DateTime ExpiresAt { get; private set; }
    public bool Used { get; private set; }
    public PasswordResetRequest(int userId, string hash) { UserId = userId; TokenHash = hash; ExpiresAt = DateTime.UtcNow.AddMinutes(30); }
    public void Consume() { Guard.Require(!Used && ExpiresAt > DateTime.UtcNow, "invalid-reset-token"); Used = true; }
}
