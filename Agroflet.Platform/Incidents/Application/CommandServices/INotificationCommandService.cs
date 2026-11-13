using Agroflet.Platform.Incidents.Domain.Model.Commands;
using Agroflet.Platform.Incidents.Domain.Model.Aggregates;
namespace Agroflet.Platform.Incidents.Application.CommandServices;

public interface INotificationCommandService
{

    Task<Result<Notification>> Handle(UpdateNotificationCommand command, CancellationToken ct = default);
}
