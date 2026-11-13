using Agroflet.Platform.Shipments.Domain.Model.Aggregates;
using Agroflet.Platform.Shipments.Domain.Model.Commands;
using Agroflet.Platform.Shipments.Domain.Repositories;
using Agroflet.Platform.Shipments.Application.CommandServices;
using Agroflet.Platform.Shipments.Application.QueryServices;
using Agroflet.Platform.Fleet.Interfaces.Acl;
using Agroflet.Platform.Iam.Interfaces.Acl;
using Agroflet.Platform.Shared.Application.Internal.OutboundServices;
using Agroflet.Platform.Shared.Domain.Repositories;
namespace Agroflet.Platform.Shipments.Application.Internal.CommandServices;

public class ShipmentCommandService(IShipmentRepository shipments, IFleetContextFacade fleet,
    IIamContextFacade iam, ILocationRepository locations, ICurrentUser user, IUnitOfWork unitOfWork) : IShipmentCommandService
{
    public async Task<Result<Shipment>> Handle(CreateShipmentCommand command, CancellationToken ct = default)
    {
        return await Result<Shipment>.ExecuteAsync(async () =>
        {
            user.RequireDispatcher();
            return await unitOfWork.ExecuteAsync(async () =>
            {
                await fleet.ValidateAllocation(command.VehicleId, command.DriverId, user.Id, command.WeightTons, ct);
                await iam.RequireBuyer(command.BuyerId, ct);
                var origin = await locations.FindByIdAsync(command.OriginId, ct);
                var destination = await locations.FindByIdAsync(command.DestinationId, ct);
                Guard.Require(origin?.Kind == "origin" && destination?.Kind == "destination", "invalid-locations");
                var entity = new Shipment(command with
                {
                    DispatcherId = user.Id,
                    Status = "planned",
                    Code = Guid.NewGuid().ToString("N"),
                    ActualDepartureAt = null,
                    DeliveredAt = null,
                    CancelledAt = null,
                    CancellationReason = null,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                });
                await shipments.AddAsync(entity, ct);
                await unitOfWork.CompleteAsync(ct);
                entity.AssignCode();
                return entity;
            }, ct);

        });
    }
    public async Task<Result<Shipment>> Handle(UpdateShipmentCommand command, CancellationToken ct = default)
    {
        return await Result<Shipment>.ExecuteAsync(async () =>
        {
            user.RequireDispatcher();
            return await unitOfWork.ExecuteAsync(async () =>
            {
                var entity = await shipments.FindByIdAsync(command.Id, ct) ?? throw new BusinessException("not-found", 404);
                if (entity.DispatcherId != user.Id) throw new BusinessException("not-found", 404);
                if (command.ExpectedVersion.HasValue && command.ExpectedVersion.Value != entity.Version) throw new BusinessException("concurrent-update-retry", 409);
                if (command.Status == null) entity.Touch();
                else entity.Transition(command.Status, command.CancellationReason);
                return entity;
            }, ct);

        });
    }
}
