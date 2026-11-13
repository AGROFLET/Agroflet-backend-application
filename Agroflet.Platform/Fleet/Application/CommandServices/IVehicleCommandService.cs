using Agroflet.Platform.Fleet.Domain.Model.Commands;
using Agroflet.Platform.Fleet.Domain.Model.Aggregates;
namespace Agroflet.Platform.Fleet.Application.CommandServices;

public interface IVehicleCommandService
{
    Task<Result<Vehicle>> Handle(CreateVehicleCommand command, CancellationToken ct = default);
    Task<Result<Vehicle>> Handle(UpdateVehicleCommand command, CancellationToken ct = default);
}
