using Agroflet.Platform.Tracking.Domain.Model.Queries;
using Agroflet.Platform.Tracking.Application.QueryServices;
using Agroflet.Platform.Tracking.Application.Internal.OutboundServices;
using Agroflet.Platform.Shipments.Interfaces.Acl;
using Agroflet.Platform.Shared.Application.Internal.OutboundServices;
namespace Agroflet.Platform.Tracking.Application.Internal.QueryServices;

public sealed class PlannedRouteQueryService(IShipmentsContextFacade shipments, ICurrentUser user, IRouteProvider provider) : IPlannedRouteQueryService
{
    public Task<Result<PlannedRouteResult>> Handle(GetPlannedRouteQuery query, CancellationToken ct) => Result<PlannedRouteResult>.ExecuteAsync(async () =>
    {
        var route = await shipments.GetRouteEndpoints(query.ShipmentId, user.Id, ct);
        return await provider.Get(route.OriginLatitude, route.OriginLongitude, route.DestinationLatitude, route.DestinationLongitude, ct);
    });
}
