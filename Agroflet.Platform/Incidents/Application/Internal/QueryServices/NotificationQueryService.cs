using Agroflet.Platform.Incidents.Domain.Model.Aggregates;
using Agroflet.Platform.Incidents.Domain.Repositories;
using Agroflet.Platform.Incidents.Application.CommandServices;
using Agroflet.Platform.Incidents.Application.QueryServices;
using Agroflet.Platform.Incidents.Application.Internal.OutboundServices;
using Agroflet.Platform.Shared.Application.Internal.OutboundServices;
using Agroflet.Platform.Shared.Domain.Model.Queries;
using Agroflet.Platform.Shared.Application.Patterns;
using Agroflet.Platform.Incidents.Domain.Model.Queries;
namespace Agroflet.Platform.Incidents.Application.Internal.QueryServices;

public class NotificationQueryService(INotificationRepository repository, ICurrentUser user) : INotificationQueryService
{
    public async Task<Result<IReadOnlyList<Notification>>> Handle(CollectionQuery query, CancellationToken ct = default)
    {
        return await Result<IReadOnlyList<Notification>>.ExecuteAsync(async () =>
        {

            var entities = await repository.FindAsync(e => e.RecipientId == user.Id, ct);

            return CollectionFilter.Apply(entities, query);

        });
    }
    public async Task<Result<Notification>> Handle(GetNotificationByIdQuery query, CancellationToken ct = default)
    {
        return await Result<Notification>.ExecuteAsync(async () =>
        {

            return (await repository.FindAsync(e => e.Id == query.Id && e.RecipientId == user.Id, ct)).SingleOrDefault() ?? throw new BusinessException("not-found", 404);

        });
    }
}
