using Agroflet.Platform.Shipments.Domain.Model.Aggregates;
using Agroflet.Platform.Shipments.Domain.Model.Queries;
using Agroflet.Platform.Shared.Domain.Model.Queries;
namespace Agroflet.Platform.Shipments.Application.QueryServices;

public interface IStatusChangeQueryService
{
    Task<Result<IReadOnlyList<StatusChange>>> Handle(CollectionQuery query, CancellationToken ct = default);
    Task<Result<StatusChange>> Handle(GetStatusChangeByIdQuery query, CancellationToken ct = default);
}
