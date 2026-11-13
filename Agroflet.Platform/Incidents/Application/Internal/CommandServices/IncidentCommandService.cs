using Agroflet.Platform.Incidents.Application.CommandServices;
using Agroflet.Platform.Incidents.Application.QueryServices;
using Agroflet.Platform.Incidents.Application.Internal.OutboundServices;
using Agroflet.Platform.Incidents.Domain.Repositories;
using Agroflet.Platform.Incidents.Domain.Model.Aggregates;
using Agroflet.Platform.Incidents.Domain.Model.Commands;
using Agroflet.Platform.Shipments.Interfaces.Acl;
using Agroflet.Platform.Shared.Application.Internal.OutboundServices;
using Agroflet.Platform.Shared.Domain.Repositories;
namespace Agroflet.Platform.Incidents.Application.Internal.CommandServices;

public class IncidentCommandService(IIncidentRepository repository, IShipmentsContextFacade shipments, ICurrentUser user, IUnitOfWork unitOfWork) : IIncidentCommandService
{
    public async Task<Result<Incident>> Handle(CreateIncidentCommand command, CancellationToken ct = default)
    {
        return await Result<Incident>.ExecuteAsync(async () =>
        {
            user.RequireDispatcher();
            command = command with { OccurredAt = UtcTimestamp.Normalize(command.OccurredAt) };
            return await unitOfWork.ExecuteAsync(async () =>
            {
                await shipments.RequireOwned(command.ShipmentId, user.Id, ct);
                var existing = (await repository.FindAsync(e => e.ShipmentId == command.ShipmentId && e.IdempotencyKey == command.IdempotencyKey, ct)).SingleOrDefault();
                if (existing != null)
                {
                    if (existing.Type != command.Type || existing.Description != command.Description || existing.EstimatedDelayMinutes != command.EstimatedDelayMinutes || existing.OccurredAt != command.OccurredAt) throw new BusinessException("idempotency-key-reused", 409);
                    return existing;
                }
                await shipments.RequireOwnedTransit(command.ShipmentId, user.Id, ct);
                var entity = new Incident(command with { ReportedBy = user.Id, CreatedAt = DateTime.UtcNow });
                await repository.AddAsync(entity, ct); await unitOfWork.CompleteAsync(ct);
                entity.RecordReported();
                return entity;
            }, ct);

        });
    }
}
