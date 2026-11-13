namespace Agroflet.Platform.Incidents.Application.Internal.OutboundServices;

public interface INotificationPublisher { Task Publish(int shipmentId, string code, int dispatcherId, int buyerId, string type, string key, object payload, CancellationToken ct); }
