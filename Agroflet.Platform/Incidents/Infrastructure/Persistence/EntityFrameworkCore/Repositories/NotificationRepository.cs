using Agroflet.Platform.Incidents.Domain.Model.Aggregates;
using Agroflet.Platform.Incidents.Domain.Repositories;
using Agroflet.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using Agroflet.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Microsoft.EntityFrameworkCore;
namespace Agroflet.Platform.Incidents.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class NotificationRepository(AppDbContext context) : BaseRepository<Notification>(context), INotificationRepository
{
    public async Task<IReadOnlyList<Notification>> FindAsync(System.Linq.Expressions.Expression<Func<Notification, bool>> predicate, CancellationToken cancellationToken = default)
        => await Context.Set<Notification>().Where(predicate).ToListAsync(cancellationToken);
}
