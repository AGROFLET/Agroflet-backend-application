using Agroflet.Platform.Shipments.Domain.Model.Aggregates;
using Agroflet.Platform.Shipments.Domain.Repositories;
using Agroflet.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using Agroflet.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Microsoft.EntityFrameworkCore;
namespace Agroflet.Platform.Shipments.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class LocationRepository(AppDbContext context) : BaseRepository<Location>(context), ILocationRepository
{
    public async Task<IReadOnlyList<Location>> FindAsync(System.Linq.Expressions.Expression<Func<Location, bool>> predicate, CancellationToken cancellationToken = default)
        => await Context.Set<Location>().Where(predicate).ToListAsync(cancellationToken);
}
