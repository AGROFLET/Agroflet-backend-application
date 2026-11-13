using Agroflet.Platform.Incidents.Application.CommandServices;
using Agroflet.Platform.Incidents.Application.QueryServices;
using Agroflet.Platform.Incidents.Application.Internal.OutboundServices;
using Agroflet.Platform.Incidents.Domain.Repositories;
using Agroflet.Platform.Incidents.Domain.Model.Aggregates;
using Agroflet.Platform.Incidents.Domain.Model.Commands;
using Agroflet.Platform.Shared.Application.Internal.OutboundServices;
using Agroflet.Platform.Shared.Domain.Repositories;
namespace Agroflet.Platform.Incidents.Application.Internal.CommandServices;

public class NotificationCommandService(INotificationRepository repository, ICurrentUser user, IUnitOfWork unitOfWork) : INotificationCommandService
{
    public async Task<Result<Notification>> Handle(UpdateNotificationCommand command, CancellationToken ct = default)
    {
        return await Result<Notification>.ExecuteAsync(async () =>
        {
            var entity = await repository.FindByIdAsync(command.Id, ct) ?? throw new BusinessException("not-found", 404);
            if (entity.RecipientId != user.Id) throw new BusinessException("not-found", 404);
            if (command.IsRead != true) throw new ArgumentException("is-read-required");
            entity.MarkRead(); await unitOfWork.CompleteAsync(ct); return entity;

        });
    }
}
