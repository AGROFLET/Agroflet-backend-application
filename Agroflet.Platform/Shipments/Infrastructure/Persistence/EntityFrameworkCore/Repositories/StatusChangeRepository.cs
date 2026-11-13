using Agroflet.Platform.Shipments.Domain.Model.Aggregates;
using Agroflet.Platform.Shipments.Domain.Repositories;
using Agroflet.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using Agroflet.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Microsoft.EntityFrameworkCore;
namespace Agroflet.Platform.Shipments.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class StatusChangeRepository(AppDbContext context) : BaseRepository<StatusChange>(context), IStatusChangeRepository
{
    public async Task<IReadOnlyList<StatusChange>> FindAsync(System.Linq.Expressions.Expression<Func<StatusChange, bool>> predicate, CancellationToken cancellationToken = default)
        => await Context.Set<StatusChange>().Where(predicate).ToListAsync(cancellationToken);
}
