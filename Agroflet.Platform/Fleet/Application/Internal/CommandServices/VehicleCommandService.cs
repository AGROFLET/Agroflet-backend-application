using Agroflet.Platform.Fleet.Domain.Model.Aggregates;
using Agroflet.Platform.Fleet.Domain.Model.Commands;
using Agroflet.Platform.Fleet.Domain.Repositories;
using Agroflet.Platform.Fleet.Application.CommandServices;
using Agroflet.Platform.Fleet.Application.QueryServices;
using Agroflet.Platform.Shared.Application.Internal.OutboundServices;
using Agroflet.Platform.Shared.Domain.Repositories;
namespace Agroflet.Platform.Fleet.Application.Internal.CommandServices;

public class VehicleCommandService(IVehicleRepository repository, ICurrentUser user, IUnitOfWork unitOfWork) : IVehicleCommandService
{
    public async Task<Result<Vehicle>> Handle(CreateVehicleCommand command, CancellationToken ct = default)
    {
        return await Result<Vehicle>.ExecuteAsync(async () =>
        {
            user.RequireDispatcher();
            var entity = new Vehicle(command with { DispatcherId = user.Id, Status = "available", UpdatedAt = DateTime.UtcNow });
            await repository.AddAsync(entity, ct); await unitOfWork.CompleteAsync(ct); return entity;

        });
    }
    public async Task<Result<Vehicle>> Handle(UpdateVehicleCommand command, CancellationToken ct = default)
    {
        return await Result<Vehicle>.ExecuteAsync(async () =>
        {
            user.RequireDispatcher();
            var entity = await repository.FindByIdAsync(command.Id, ct) ?? throw new BusinessException("not-found", 404);
            if (entity.DispatcherId != user.Id) throw new BusinessException("not-found", 404);
            if (entity.Status is "reserved" or "in_route") throw new BusinessException("status-locked", 409);
            if (command.Status != null) entity.SetOperationalStatus(Guard.OneOf(command.Status, "available", "under_maintenance"));
            entity.Edit(new CreateVehicleCommand(entity.DispatcherId, command.Plate ?? entity.Plate, command.Brand ?? entity.Brand, command.Model ?? entity.Model, command.Year ?? entity.Year, command.CapacityTons ?? entity.CapacityTons, command.BodyType ?? entity.BodyType, entity.Status, DateTime.UtcNow));
            await unitOfWork.CompleteAsync(ct); return entity;

        });
    }
}
