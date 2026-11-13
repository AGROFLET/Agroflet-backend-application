using Agroflet.Platform.Fleet.Domain.Model.Aggregates;
using Agroflet.Platform.Fleet.Domain.Model.Queries;
using Agroflet.Platform.Shared.Domain.Model.Queries;
namespace Agroflet.Platform.Fleet.Application.QueryServices;

public interface IVehicleQueryService
{
    Task<Result<IReadOnlyList<Vehicle>>> Handle(CollectionQuery query, CancellationToken ct = default);
    Task<Result<Vehicle>> Handle(GetVehicleByIdQuery query, CancellationToken ct = default);
}
