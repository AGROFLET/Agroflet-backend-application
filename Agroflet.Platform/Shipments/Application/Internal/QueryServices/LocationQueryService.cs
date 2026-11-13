using Agroflet.Platform.Shipments.Domain.Model.Aggregates;
using Agroflet.Platform.Shipments.Domain.Repositories;
using Agroflet.Platform.Shipments.Application.CommandServices;
using Agroflet.Platform.Shipments.Application.QueryServices;
using Agroflet.Platform.Shared.Application.Internal.OutboundServices;
using Agroflet.Platform.Shared.Domain.Model.Queries;
using Agroflet.Platform.Shared.Application.Patterns;
using Agroflet.Platform.Shipments.Domain.Model.Queries;
namespace Agroflet.Platform.Shipments.Application.Internal.QueryServices;

public class LocationQueryService(ILocationRepository repository) : ILocationQueryService
{
    public async Task<Result<IReadOnlyList<Location>>> Handle(CollectionQuery query, CancellationToken ct = default)
    {
        return await Result<IReadOnlyList<Location>>.ExecuteAsync(async () =>
        {

            var entities = await repository.FindAsync(e => true, ct);

            return CollectionFilter.Apply(entities, query);

        });
    }
    public async Task<Result<Location>> Handle(GetLocationByIdQuery query, CancellationToken ct = default)
    {
        return await Result<Location>.ExecuteAsync(async () =>
        {

            return (await repository.FindAsync(e => e.Id == query.Id && true, ct)).SingleOrDefault() ?? throw new BusinessException("not-found", 404);

        });
    }
}
