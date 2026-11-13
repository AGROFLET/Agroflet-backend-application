namespace Agroflet.Platform.Fleet.Interfaces.Acl;
/// <summary>Operations owned by Fleet; no domain entities cross this boundary.</summary>
public interface IFleetContextFacade
{
    Task ValidateAllocation(int vehicleId, int driverId, int dispatcherId, decimal weight, CancellationToken ct);
    Task ApplyShipmentState(int vehicleId, int driverId, int dispatcherId, string state, CancellationToken ct);
}
