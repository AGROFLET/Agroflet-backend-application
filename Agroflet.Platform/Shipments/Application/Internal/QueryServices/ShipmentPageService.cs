using Agroflet.Platform.Shipments.Application.QueryServices;
using Agroflet.Platform.Shipments.Domain.Model.Queries;
using Agroflet.Platform.Shipments.Domain.Repositories;
using Agroflet.Platform.Shared.Application.Internal.OutboundServices;
namespace Agroflet.Platform.Shipments.Application.Internal.QueryServices;

public sealed class ShipmentPageService(IShipmentPageRepository repository, ICurrentUser user) : IShipmentPageService
{
    public Task<Result<ShipmentPage>> Handle(GetShipmentPageQuery query, CancellationToken ct)
        => Result<ShipmentPage>.ExecuteAsync(() => repository.Find(query, user.Id, ct));
}
