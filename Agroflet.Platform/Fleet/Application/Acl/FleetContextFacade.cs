using Agroflet.Platform.Fleet.Interfaces.Acl;
using Agroflet.Platform.Fleet.Domain.Repositories;
namespace Agroflet.Platform.Fleet.Application.Acl;

public class FleetContextFacade(IVehicleRepository vehicles, IDriverRepository drivers) : IFleetContextFacade
{
    public async Task ValidateAllocation(int vehicleId, int driverId, int dispatcherId, decimal weight, CancellationToken ct)
    {
        var vehicle = await vehicles.FindByIdAsync(vehicleId, ct) ?? throw new BusinessException("vehicle-not-found", 404);
        var driver = await drivers.FindByIdAsync(driverId, ct) ?? throw new BusinessException("driver-not-found", 404);
        if (vehicle.DispatcherId != dispatcherId || driver.DispatcherId != dispatcherId) throw new BusinessException("not-found", 404);
        if (vehicle.Status != "available" || driver.Status != "available") throw new BusinessException("resources-unavailable", 409);
        if (weight > vehicle.CapacityTons) throw new BusinessException("weight-exceeds-capacity", 422);
    }

    public async Task ApplyShipmentState(int vehicleId, int driverId, int dispatcherId, string state, CancellationToken ct)
    {
        var vehicle = await vehicles.FindByIdAsync(vehicleId, ct) ?? throw new BusinessException("vehicle-not-found", 404);
        var driver = await drivers.FindByIdAsync(driverId, ct) ?? throw new BusinessException("driver-not-found", 404);
        if (vehicle.DispatcherId != dispatcherId || driver.DispatcherId != dispatcherId) throw new BusinessException("not-found", 404);
        if (state == "planned" && (vehicle.Status != "available" || driver.Status != "available"))
            throw new BusinessException("resources-unavailable", 409);
        vehicle.SetOperationalStatus(state == "planned" ? "reserved" : state == "in_transit" ? "in_route" : "available");
        driver.SetOperationalStatus(state is "planned" or "in_transit" ? "assigned" : "available");
    }
}
