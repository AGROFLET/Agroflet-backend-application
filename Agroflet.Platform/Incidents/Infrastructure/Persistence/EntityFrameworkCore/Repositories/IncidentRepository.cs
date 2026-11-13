using Agroflet.Platform.Incidents.Domain.Model.Aggregates;
using Agroflet.Platform.Incidents.Domain.Repositories;
using Agroflet.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using Agroflet.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Microsoft.EntityFrameworkCore;
namespace Agroflet.Platform.Incidents.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class IncidentRepository(AppDbContext context) : BaseRepository<Incident>(context), IIncidentRepository
{
    public async Task<IReadOnlyList<Incident>> FindAsync(System.Linq.Expressions.Expression<Func<Incident, bool>> predicate, CancellationToken cancellationToken = default)
        => await Context.Set<Incident>().Where(predicate).ToListAsync(cancellationToken);
}
