namespace Agroflet.Platform.Incidents.Interfaces.Acl;

public interface IIncidentsContextFacade
{
    Task NotifyShipmentStatus(int shipmentId, string code, int dispatcherId, int buyerId,
        int statusChangeId, string? previousStatus, string status, CancellationToken ct);
}
