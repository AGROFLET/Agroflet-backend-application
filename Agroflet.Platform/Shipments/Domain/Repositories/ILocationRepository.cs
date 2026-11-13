using Agroflet.Platform.Shipments.Domain.Model.Aggregates;
using Agroflet.Platform.Shared.Domain.Repositories;
namespace Agroflet.Platform.Shipments.Domain.Repositories;

public interface ILocationRepository : IBaseRepository<Location>
{
    Task<IReadOnlyList<Location>> FindAsync(System.Linq.Expressions.Expression<Func<Location, bool>> predicate, CancellationToken cancellationToken = default);
}
