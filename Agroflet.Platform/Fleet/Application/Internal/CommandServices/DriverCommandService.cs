using Agroflet.Platform.Fleet.Domain.Model.Aggregates;
using Agroflet.Platform.Fleet.Domain.Model.Commands;
using Agroflet.Platform.Fleet.Domain.Repositories;
using Agroflet.Platform.Fleet.Application.CommandServices;
using Agroflet.Platform.Fleet.Application.QueryServices;
using Agroflet.Platform.Shared.Application.Internal.OutboundServices;
using Agroflet.Platform.Shared.Domain.Repositories;
namespace Agroflet.Platform.Fleet.Application.Internal.CommandServices;

public class DriverCommandService(IDriverRepository repository, ICurrentUser user, IUnitOfWork unitOfWork) : IDriverCommandService
{
    public async Task<Result<Driver>> Handle(CreateDriverCommand command, CancellationToken ct = default)
    {
        return await Result<Driver>.ExecuteAsync(async () =>
        {
            user.RequireDispatcher();
            var entity = new Driver(command with { DispatcherId = user.Id, Status = "available", UpdatedAt = DateTime.UtcNow });
            await repository.AddAsync(entity, ct); await unitOfWork.CompleteAsync(ct); return entity;

        });
    }
    public async Task<Result<Driver>> Handle(UpdateDriverCommand command, CancellationToken ct = default)
    {
        return await Result<Driver>.ExecuteAsync(async () =>
        {
            user.RequireDispatcher();
            var entity = await repository.FindByIdAsync(command.Id, ct) ?? throw new BusinessException("not-found", 404);
            if (entity.DispatcherId != user.Id) throw new BusinessException("not-found", 404);
            if (entity.Status == "assigned") throw new BusinessException("status-locked", 409);
            if (command.Status != null && command.Status != "available") throw new BusinessException("status-locked", 409);
            entity.Edit(new CreateDriverCommand(entity.DispatcherId, command.FirstName ?? entity.FirstName, command.LastName ?? entity.LastName, command.Dni ?? entity.Dni, command.LicenseNumber ?? entity.LicenseNumber, command.LicenseCategory ?? entity.LicenseCategory, command.Phone ?? entity.Phone, entity.Status, DateTime.UtcNow));
            await unitOfWork.CompleteAsync(ct); return entity;

        });
    }
}
