using Agroflet.Platform.Tracking.Domain.Model.Aggregates;
using Agroflet.Platform.Tracking.Domain.Repositories;
using Agroflet.Platform.Tracking.Application.CommandServices;
using Agroflet.Platform.Tracking.Application.QueryServices;
using Agroflet.Platform.Tracking.Application.Internal.OutboundServices;
using Agroflet.Platform.Shared.Application.Internal.OutboundServices;
using Agroflet.Platform.Shared.Domain.Model.Queries;
using Agroflet.Platform.Shared.Application.Patterns;
using Agroflet.Platform.Tracking.Domain.Model.Queries;
namespace Agroflet.Platform.Tracking.Application.Internal.QueryServices;

public class ReportedPositionQueryService(IReportedPositionRepository repository, IAccessScope access) : IReportedPositionQueryService
{
    public async Task<Result<IReadOnlyList<ReportedPosition>>> Handle(CollectionQuery query, CancellationToken ct = default)
    {
        return await Result<IReadOnlyList<ReportedPosition>>.ExecuteAsync(async () =>
        {
            var ids = await access.ShipmentIds(ct);
            var entities = await repository.FindAsync(e => ids.Contains(e.ShipmentId), ct);

            return CollectionFilter.Apply(entities, query);

        });
    }
    public async Task<Result<ReportedPosition>> Handle(GetReportedPositionByIdQuery query, CancellationToken ct = default)
    {
        return await Result<ReportedPosition>.ExecuteAsync(async () =>
        {
            var ids = await access.ShipmentIds(ct);
            return (await repository.FindAsync(e => e.Id == query.Id && ids.Contains(e.ShipmentId), ct)).SingleOrDefault() ?? throw new BusinessException("not-found", 404);

        });
    }
}
