using Agroflet.Platform.Tracking.Domain.Model.Queries;
using Agroflet.Platform.Tracking.Application.Internal.OutboundServices;
namespace Agroflet.Platform.Tracking.Application.QueryServices;

public interface IPlannedRouteQueryService { Task<Result<PlannedRouteResult>> Handle(GetPlannedRouteQuery query, CancellationToken ct); }
