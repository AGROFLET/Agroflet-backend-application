using Agroflet.Platform.Incidents.Domain.Model.Aggregates;
using Agroflet.Platform.Incidents.Interfaces.Rest.Resources;
namespace Agroflet.Platform.Incidents.Interfaces.Rest.Transform;

public static class IncidentResourceFromEntityAssembler
{
    public static IncidentResource ToResourceFromEntity(Incident entity) => new(entity.Id, entity.ShipmentId, entity.Type, entity.Description, entity.EstimatedDelayMinutes, entity.OccurredAt, entity.ReportedBy, entity.IdempotencyKey, entity.CreatedAt);
}
