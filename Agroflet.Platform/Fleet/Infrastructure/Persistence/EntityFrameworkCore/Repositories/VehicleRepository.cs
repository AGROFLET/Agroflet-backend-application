using Agroflet.Platform.Fleet.Domain.Model.Aggregates;
using Agroflet.Platform.Fleet.Domain.Repositories;
using Agroflet.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using Agroflet.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Microsoft.EntityFrameworkCore;
namespace Agroflet.Platform.Fleet.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class VehicleRepository(AppDbContext context) : BaseRepository<Vehicle>(context), IVehicleRepository
{
    public async Task<IReadOnlyList<Vehicle>> FindAsync(System.Linq.Expressions.Expression<Func<Vehicle, bool>> predicate, CancellationToken cancellationToken = default)
        => await Context.Set<Vehicle>().Where(predicate).ToListAsync(cancellationToken);
}
