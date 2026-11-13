using Agroflet.Platform.Fleet.Domain.Model.Aggregates;
using Agroflet.Platform.Shared.Domain.Repositories;
namespace Agroflet.Platform.Fleet.Domain.Repositories;

public interface IVehicleRepository : IBaseRepository<Vehicle>
{
    Task<IReadOnlyList<Vehicle>> FindAsync(System.Linq.Expressions.Expression<Func<Vehicle, bool>> predicate, CancellationToken cancellationToken = default);
}
