using Agroflet.Platform.Shipments.Domain.Model.Queries;
namespace Agroflet.Platform.Shipments.Domain.Repositories;

public interface IShipmentPageRepository { Task<ShipmentPage> Find(GetShipmentPageQuery query, int userId, CancellationToken ct); }
