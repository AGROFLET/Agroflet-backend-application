using System.Text.Json;
using Agroflet.Platform.Incidents.Application.CommandServices;
using Agroflet.Platform.Incidents.Application.QueryServices;
using Agroflet.Platform.Incidents.Application.Internal.OutboundServices;
using Agroflet.Platform.Incidents.Domain.Repositories;
using Agroflet.Platform.Incidents.Domain.Model.Aggregates;
using Agroflet.Platform.Incidents.Domain.Model.Commands;
namespace Agroflet.Platform.Incidents.Application.Internal.CommandServices;

public class NotificationPublisher(INotificationRepository repository) : INotificationPublisher
{
    public async Task Publish(int shipmentId, string code, int dispatcherId, int buyerId, string type, string key, object payload, CancellationToken ct)
    {
        foreach (var id in new[] { dispatcherId, buyerId }.Distinct())
            await repository.AddAsync(new Notification(new CreateNotificationCommand(id, shipmentId, code, type, key, JsonSerializer.Serialize(payload), DateTime.UtcNow, false, null)), ct);
    }
}
