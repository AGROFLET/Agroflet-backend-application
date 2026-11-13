using Microsoft.EntityFrameworkCore;
using Agroflet.Platform.Iam.Application.CommandServices;
using Agroflet.Platform.Iam.Application.QueryServices;
using Agroflet.Platform.Iam.Application.Internal.OutboundServices;
using Agroflet.Platform.Iam.Domain.Model.Aggregates;
using Agroflet.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
namespace Agroflet.Platform.Iam.Infrastructure.Authentication;

public class SessionRevocationService(AppDbContext db) : ISessionRevocationService
{
    public Task<bool> IsRevoked(string jti, CancellationToken ct) => db.Set<RevokedSession>().AnyAsync(s => s.Jti == jti, ct);
    public async Task Revoke(string jti, int userId, DateTime expiresAt, CancellationToken ct)
    {
        if (await IsRevoked(jti, ct)) return;
        db.Add(new RevokedSession(jti, userId, expiresAt));
        try { await db.SaveChangesAsync(ct); }
        catch (DbUpdateException) { db.ChangeTracker.Clear(); if (!await IsRevoked(jti, ct)) throw; }
    }
}
