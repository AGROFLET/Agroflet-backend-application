using Agroflet.Platform.Incidents.Domain.Model.Aggregates;
using Agroflet.Platform.Shared.Domain.Repositories;
namespace Agroflet.Platform.Incidents.Domain.Repositories;

public interface INotificationRepository : IBaseRepository<Notification>
{
    Task<IReadOnlyList<Notification>> FindAsync(System.Linq.Expressions.Expression<Func<Notification, bool>> predicate, CancellationToken cancellationToken = default);
}
