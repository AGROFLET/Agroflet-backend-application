namespace Agroflet.Platform.Shared.Application.Internal.OutboundServices;

public interface IAccessScope
{
    Task<int[]> ParticipantIds(CancellationToken ct);
    Task<int[]> ShipmentIds(CancellationToken ct);
    Task<int[]> VehicleIds(CancellationToken ct);
    Task<int[]> DriverIds(CancellationToken ct);
}
