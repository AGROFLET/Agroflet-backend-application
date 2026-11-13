using Agroflet.Platform.Tracking.Application.CommandServices;
using Agroflet.Platform.Tracking.Application.QueryServices;
using Agroflet.Platform.Tracking.Application.Internal.OutboundServices;
using Agroflet.Platform.Tracking.Domain.Repositories;
using Agroflet.Platform.Tracking.Domain.Model.Aggregates;
using Agroflet.Platform.Tracking.Domain.Model.Commands;
using Agroflet.Platform.Shipments.Interfaces.Acl;
using Agroflet.Platform.Shared.Application.Internal.OutboundServices;
using Agroflet.Platform.Shared.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
namespace Agroflet.Platform.Tracking.Application.Internal.CommandServices;

public class ReportedPositionCommandService(IReportedPositionRepository repository, IShipmentsContextFacade shipments, ICurrentUser user, IUnitOfWork unitOfWork) : IReportedPositionCommandService
{
    private async Task<ReportedPosition?> Existing(CreateReportedPositionCommand c, CancellationToken ct)
    {
        if (c.EventId == null) return null;
        var found = (await repository.FindAsync(p => p.ShipmentId == c.ShipmentId && p.EventId == c.EventId, ct)).SingleOrDefault();
        if (found != null && (found.Latitude != c.Latitude || found.Longitude != c.Longitude || found.ReportedAt != UtcTimestamp.Normalize(c.ReportedAt) || found.Source != c.Source || found.Note != c.Note || found.RecordedBy != user.Id))
            throw new BusinessException("idempotency-conflict", 409);
        return found;
    }
    public async Task<Result<ReportedPosition>> Handle(CreateReportedPositionCommand command, CancellationToken ct = default)
    {
        return await Result<ReportedPosition>.ExecuteAsync(async () =>
        {
            user.RequireDispatcher();
            await shipments.RequireOwned(command.ShipmentId, user.Id, ct);
            var prior = await Existing(command, ct); if (prior != null) return prior;
            try
            {
                return await unitOfWork.ExecuteAsync(async () =>
                {
                    await shipments.RequireOwnedTransit(command.ShipmentId, user.Id, ct);
                    var entity = new ReportedPosition(command with { RecordedBy = user.Id, RecordedAt = DateTime.UtcNow, IsDemo = false });
                    await shipments.TouchTransit(command.ShipmentId, user.Id, ct);
                    await repository.AddAsync(entity, ct); return entity;
                }, ct);
            }
            catch (DbUpdateException)
            {
                var existing = await Existing(command, ct); if (existing != null) return existing; throw;
            }

        });
    }
}
