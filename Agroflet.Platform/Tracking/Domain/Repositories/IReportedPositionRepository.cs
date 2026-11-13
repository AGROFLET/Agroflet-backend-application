using Agroflet.Platform.Tracking.Domain.Model.Aggregates;
using Agroflet.Platform.Shared.Domain.Repositories;
namespace Agroflet.Platform.Tracking.Domain.Repositories;

public interface IReportedPositionRepository : IBaseRepository<ReportedPosition>
{
    Task<IReadOnlyList<ReportedPosition>> FindAsync(System.Linq.Expressions.Expression<Func<ReportedPosition, bool>> predicate, CancellationToken cancellationToken = default);
}
