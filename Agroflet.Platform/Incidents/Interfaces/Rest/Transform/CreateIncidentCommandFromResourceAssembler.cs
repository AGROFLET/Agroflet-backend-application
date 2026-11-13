using Agroflet.Platform.Incidents.Domain.Model.Commands;
using Agroflet.Platform.Incidents.Interfaces.Rest.Resources;
namespace Agroflet.Platform.Incidents.Interfaces.Rest.Transform;

public static class CreateIncidentCommandFromResourceAssembler
{
    public static CreateIncidentCommand ToCommandFromResource(CreateIncidentResource resource, int actorId) => new(resource.ShipmentId, resource.Type, resource.Description, resource.EstimatedDelayMinutes, resource.OccurredAt, actorId, resource.IdempotencyKey, DateTime.UtcNow);
}
