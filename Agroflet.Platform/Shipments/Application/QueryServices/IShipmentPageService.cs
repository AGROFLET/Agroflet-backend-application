using Agroflet.Platform.Shipments.Domain.Model.Queries;
namespace Agroflet.Platform.Shipments.Application.QueryServices;

public interface IShipmentPageService { Task<Result<ShipmentPage>> Handle(GetShipmentPageQuery query, CancellationToken ct); }
