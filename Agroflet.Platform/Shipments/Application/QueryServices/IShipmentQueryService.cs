using Agroflet.Platform.Shipments.Domain.Model.Aggregates;
using Agroflet.Platform.Shipments.Domain.Model.Queries;
using Agroflet.Platform.Shared.Domain.Model.Queries;
namespace Agroflet.Platform.Shipments.Application.QueryServices;

public interface IShipmentQueryService
{
    Task<Result<IReadOnlyList<Shipment>>> Handle(CollectionQuery query, CancellationToken ct = default);
    Task<Result<Shipment>> Handle(GetShipmentByIdQuery query, CancellationToken ct = default);
}
