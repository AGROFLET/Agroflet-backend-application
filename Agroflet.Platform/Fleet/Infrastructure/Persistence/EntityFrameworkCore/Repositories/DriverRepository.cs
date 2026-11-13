using Agroflet.Platform.Fleet.Domain.Model.Aggregates;
using Agroflet.Platform.Fleet.Domain.Repositories;
using Agroflet.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using Agroflet.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Microsoft.EntityFrameworkCore;
namespace Agroflet.Platform.Fleet.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class DriverRepository(AppDbContext context) : BaseRepository<Driver>(context), IDriverRepository
{
    public async Task<IReadOnlyList<Driver>> FindAsync(System.Linq.Expressions.Expression<Func<Driver, bool>> predicate, CancellationToken cancellationToken = default)
        => await Context.Set<Driver>().Where(predicate).ToListAsync(cancellationToken);
}
