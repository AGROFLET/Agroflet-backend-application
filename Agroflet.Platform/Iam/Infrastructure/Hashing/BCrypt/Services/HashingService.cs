using Microsoft.AspNetCore.Identity;
using Agroflet.Platform.Iam.Domain.Model.Aggregates;
using Agroflet.Platform.Iam.Application.Internal.OutboundServices;
namespace Agroflet.Platform.Iam.Infrastructure.Hashing.BCrypt.Services;

public sealed class HashingService : IHashingService
{
    private readonly PasswordHasher<User> legacyHasher = new();
    public string Hash(User user, string password) { Guard.Require(password.Length >= 8 && password.Length <= 128, "password-length"); return global::BCrypt.Net.BCrypt.EnhancedHashPassword(password, workFactor: 12); }
    public bool Verify(User user, string password)
    {
        // Enhanced BCrypt prehashes long UTF-8 passwords; legacy AV2 hashes remain readable.
        if (user.PasswordHash.StartsWith("$2")) return global::BCrypt.Net.BCrypt.EnhancedVerify(password, user.PasswordHash);
        return legacyHasher.VerifyHashedPassword(user, user.PasswordHash, password) != PasswordVerificationResult.Failed;
    }
}
