using Agroflet.Platform.Fleet.Domain.Model.Aggregates;
using Agroflet.Platform.Shared.Domain.Repositories;
namespace Agroflet.Platform.Fleet.Domain.Repositories;

public interface IDriverRepository : IBaseRepository<Driver>
{
    Task<IReadOnlyList<Driver>> FindAsync(System.Linq.Expressions.Expression<Func<Driver, bool>> predicate, CancellationToken cancellationToken = default);
}
