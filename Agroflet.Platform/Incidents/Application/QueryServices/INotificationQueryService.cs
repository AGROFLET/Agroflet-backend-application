using Agroflet.Platform.Incidents.Domain.Model.Aggregates;
using Agroflet.Platform.Incidents.Domain.Model.Queries;
using Agroflet.Platform.Shared.Domain.Model.Queries;
namespace Agroflet.Platform.Incidents.Application.QueryServices;

public interface INotificationQueryService
{
    Task<Result<IReadOnlyList<Notification>>> Handle(CollectionQuery query, CancellationToken ct = default);
    Task<Result<Notification>> Handle(GetNotificationByIdQuery query, CancellationToken ct = default);
}
