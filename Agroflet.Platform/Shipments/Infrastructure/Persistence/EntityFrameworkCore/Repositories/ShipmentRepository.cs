using Agroflet.Platform.Shipments.Domain.Model.Aggregates;
using Agroflet.Platform.Shipments.Domain.Repositories;
using Agroflet.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using Agroflet.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Microsoft.EntityFrameworkCore;
namespace Agroflet.Platform.Shipments.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class ShipmentRepository(AppDbContext context) : BaseRepository<Shipment>(context), IShipmentRepository
{
    public async Task<IReadOnlyList<Shipment>> FindAsync(System.Linq.Expressions.Expression<Func<Shipment, bool>> predicate, CancellationToken cancellationToken = default)
        => await Context.Set<Shipment>().Where(predicate).ToListAsync(cancellationToken);
}
