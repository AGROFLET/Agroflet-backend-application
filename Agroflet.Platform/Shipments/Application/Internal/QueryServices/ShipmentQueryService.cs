using Agroflet.Platform.Shipments.Domain.Model.Aggregates;
using Agroflet.Platform.Shipments.Domain.Repositories;
using Agroflet.Platform.Shipments.Application.CommandServices;
using Agroflet.Platform.Shipments.Application.QueryServices;
using Agroflet.Platform.Shared.Application.Internal.OutboundServices;
using Agroflet.Platform.Shared.Domain.Model.Queries;
using Agroflet.Platform.Shared.Application.Patterns;
using Agroflet.Platform.Shipments.Domain.Model.Queries;
namespace Agroflet.Platform.Shipments.Application.Internal.QueryServices;

public class ShipmentQueryService(IShipmentRepository repository, ICurrentUser user) : IShipmentQueryService
{
    public async Task<Result<IReadOnlyList<Shipment>>> Handle(CollectionQuery query, CancellationToken ct = default)
    {
        return await Result<IReadOnlyList<Shipment>>.ExecuteAsync(async () =>
        {

            var entities = await repository.FindAsync(e => (e.DispatcherId == user.Id || e.BuyerId == user.Id), ct);

            return CollectionFilter.Apply(entities, query);

        });
    }
    public async Task<Result<Shipment>> Handle(GetShipmentByIdQuery query, CancellationToken ct = default)
    {
        return await Result<Shipment>.ExecuteAsync(async () =>
        {

            return (await repository.FindAsync(e => e.Id == query.Id && (e.DispatcherId == user.Id || e.BuyerId == user.Id), ct)).SingleOrDefault() ?? throw new BusinessException("not-found", 404);

        });
    }
}
