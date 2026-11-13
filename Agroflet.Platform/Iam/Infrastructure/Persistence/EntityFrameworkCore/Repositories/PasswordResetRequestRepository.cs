using Microsoft.EntityFrameworkCore;
using Agroflet.Platform.Iam.Domain.Model.Aggregates;
using Agroflet.Platform.Iam.Domain.Repositories;
using Agroflet.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using Agroflet.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
namespace Agroflet.Platform.Iam.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class PasswordResetRequestRepository(AppDbContext context) : BaseRepository<PasswordResetRequest>(context), IPasswordResetRequestRepository
{ public Task<PasswordResetRequest?> FindByHashAsync(string hash, CancellationToken ct) => Context.Set<PasswordResetRequest>().SingleOrDefaultAsync(e => e.TokenHash == hash, ct); }
