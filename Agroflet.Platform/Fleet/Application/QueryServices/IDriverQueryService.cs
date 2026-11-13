using Agroflet.Platform.Fleet.Domain.Model.Aggregates;
using Agroflet.Platform.Fleet.Domain.Model.Queries;
using Agroflet.Platform.Shared.Domain.Model.Queries;
namespace Agroflet.Platform.Fleet.Application.QueryServices;

public interface IDriverQueryService
{
    Task<Result<IReadOnlyList<Driver>>> Handle(CollectionQuery query, CancellationToken ct = default);
    Task<Result<Driver>> Handle(GetDriverByIdQuery query, CancellationToken ct = default);
}
