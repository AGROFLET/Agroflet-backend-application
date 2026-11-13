using Agroflet.Platform.Shipments.Domain.Model.Aggregates;
using Agroflet.Platform.Shared.Domain.Repositories;
namespace Agroflet.Platform.Shipments.Domain.Repositories;

public interface IShipmentRepository : IBaseRepository<Shipment>
{
    Task<IReadOnlyList<Shipment>> FindAsync(System.Linq.Expressions.Expression<Func<Shipment, bool>> predicate, CancellationToken cancellationToken = default);
}
