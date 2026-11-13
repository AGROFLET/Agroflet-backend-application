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

public class IncidentQueryService(IIncidentRepository repository, IAccessScope access) : IIncidentQueryService
{
    public async Task<Result<IReadOnlyList<Incident>>> Handle(CollectionQuery query, CancellationToken ct = default)
    {
        return await Result<IReadOnlyList<Incident>>.ExecuteAsync(async () =>
        {
            var ids = await access.ShipmentIds(ct);
            var entities = await repository.FindAsync(e => ids.Contains(e.ShipmentId), ct);

            return CollectionFilter.Apply(entities, query);

        });
    }
    public async Task<Result<Incident>> Handle(GetIncidentByIdQuery query, CancellationToken ct = default)
    {
        return await Result<Incident>.ExecuteAsync(async () =>
        {
            var ids = await access.ShipmentIds(ct);
            return (await repository.FindAsync(e => e.Id == query.Id && ids.Contains(e.ShipmentId), ct)).SingleOrDefault() ?? throw new BusinessException("not-found", 404);

        });
    }
}
