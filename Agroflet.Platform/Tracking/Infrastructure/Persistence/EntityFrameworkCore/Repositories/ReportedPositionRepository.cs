using Agroflet.Platform.Tracking.Domain.Model.Aggregates;
using Agroflet.Platform.Tracking.Domain.Repositories;
using Agroflet.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using Agroflet.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Microsoft.EntityFrameworkCore;
namespace Agroflet.Platform.Tracking.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class ReportedPositionRepository(AppDbContext context) : BaseRepository<ReportedPosition>(context), IReportedPositionRepository
{
    public async Task<IReadOnlyList<ReportedPosition>> FindAsync(System.Linq.Expressions.Expression<Func<ReportedPosition, bool>> predicate, CancellationToken cancellationToken = default)
        => await Context.Set<ReportedPosition>().Where(predicate).ToListAsync(cancellationToken);
}
