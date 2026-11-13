using Agroflet.Platform.Shipments.Domain.Model.Aggregates;
using Agroflet.Platform.Shipments.Domain.Repositories;
using Agroflet.Platform.Shipments.Application.CommandServices;
using Agroflet.Platform.Shipments.Application.QueryServices;
using Agroflet.Platform.Shared.Application.Internal.OutboundServices;
using Agroflet.Platform.Shared.Domain.Model.Queries;
using Agroflet.Platform.Shared.Application.Patterns;
using Agroflet.Platform.Shipments.Domain.Model.Queries;
namespace Agroflet.Platform.Shipments.Application.Internal.QueryServices;

public class StatusChangeQueryService(IStatusChangeRepository repository, IAccessScope access) : IStatusChangeQueryService
{
    public async Task<Result<IReadOnlyList<StatusChange>>> Handle(CollectionQuery query, CancellationToken ct = default)
    {
        return await Result<IReadOnlyList<StatusChange>>.ExecuteAsync(async () =>
        {
            var ids = await access.ShipmentIds(ct);
            var entities = await repository.FindAsync(e => ids.Contains(e.ShipmentId), ct);

            return CollectionFilter.Apply(entities, query);

        });
    }
    public async Task<Result<StatusChange>> Handle(GetStatusChangeByIdQuery query, CancellationToken ct = default)
    {
        return await Result<StatusChange>.ExecuteAsync(async () =>
        {
            var ids = await access.ShipmentIds(ct);
            return (await repository.FindAsync(e => e.Id == query.Id && ids.Contains(e.ShipmentId), ct)).SingleOrDefault() ?? throw new BusinessException("not-found", 404);

        });
    }
}
