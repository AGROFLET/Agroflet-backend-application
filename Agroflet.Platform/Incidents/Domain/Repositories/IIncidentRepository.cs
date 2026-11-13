using Agroflet.Platform.Incidents.Domain.Model.Aggregates;
using Agroflet.Platform.Shared.Domain.Repositories;
namespace Agroflet.Platform.Incidents.Domain.Repositories;

public interface IIncidentRepository : IBaseRepository<Incident>
{
    Task<IReadOnlyList<Incident>> FindAsync(System.Linq.Expressions.Expression<Func<Incident, bool>> predicate, CancellationToken cancellationToken = default);
}
