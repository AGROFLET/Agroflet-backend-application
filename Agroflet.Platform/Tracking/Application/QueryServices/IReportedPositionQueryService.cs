using Agroflet.Platform.Tracking.Domain.Model.Aggregates;
using Agroflet.Platform.Tracking.Domain.Model.Queries;
using Agroflet.Platform.Shared.Domain.Model.Queries;
namespace Agroflet.Platform.Tracking.Application.QueryServices;

public interface IReportedPositionQueryService
{
    Task<Result<IReadOnlyList<ReportedPosition>>> Handle(CollectionQuery query, CancellationToken ct = default);
    Task<Result<ReportedPosition>> Handle(GetReportedPositionByIdQuery query, CancellationToken ct = default);
}
