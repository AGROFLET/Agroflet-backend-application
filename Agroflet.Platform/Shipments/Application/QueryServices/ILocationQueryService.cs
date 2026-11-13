using Agroflet.Platform.Shipments.Domain.Model.Aggregates;
using Agroflet.Platform.Shipments.Domain.Model.Queries;
using Agroflet.Platform.Shared.Domain.Model.Queries;
namespace Agroflet.Platform.Shipments.Application.QueryServices;

public interface ILocationQueryService
{
    Task<Result<IReadOnlyList<Location>>> Handle(CollectionQuery query, CancellationToken ct = default);
    Task<Result<Location>> Handle(GetLocationByIdQuery query, CancellationToken ct = default);
}
