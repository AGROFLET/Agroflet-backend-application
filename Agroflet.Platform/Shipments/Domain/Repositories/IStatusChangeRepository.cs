using Agroflet.Platform.Shipments.Domain.Model.Aggregates;
using Agroflet.Platform.Shared.Domain.Repositories;
namespace Agroflet.Platform.Shipments.Domain.Repositories;

public interface IStatusChangeRepository : IBaseRepository<StatusChange>
{
    Task<IReadOnlyList<StatusChange>> FindAsync(System.Linq.Expressions.Expression<Func<StatusChange, bool>> predicate, CancellationToken cancellationToken = default);
}
