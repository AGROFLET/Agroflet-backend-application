using Agroflet.Platform.Shared.Application.Internal.OutboundServices;
using Agroflet.Platform.Shipments.Domain.Repositories;
namespace Agroflet.Platform.Shipments.Application.Internal.QueryServices;

public class AccessScope(IShipmentRepository shipments, ICurrentUser user) : IAccessScope
{
    public async Task<int[]> ParticipantIds(CancellationToken ct) => (await shipments.FindAsync(s => s.DispatcherId == user.Id || s.BuyerId == user.Id, ct)).SelectMany(s => new[] { s.DispatcherId, s.BuyerId }).Distinct().ToArray();
    public async Task<int[]> ShipmentIds(CancellationToken ct) => (await shipments.FindAsync(s => s.DispatcherId == user.Id || s.BuyerId == user.Id, ct)).Select(s => s.Id).ToArray();
    public async Task<int[]> VehicleIds(CancellationToken ct) => (await shipments.FindAsync(s => s.BuyerId == user.Id, ct)).Select(s => s.VehicleId).ToArray();
    public async Task<int[]> DriverIds(CancellationToken ct) => (await shipments.FindAsync(s => s.BuyerId == user.Id, ct)).Select(s => s.DriverId).ToArray();
}
