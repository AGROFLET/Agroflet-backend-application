using Agroflet.Platform.Tracking.Domain.Model.Aggregates;
using Agroflet.Platform.Tracking.Interfaces.Rest.Resources;
namespace Agroflet.Platform.Tracking.Interfaces.Rest.Transform;

public static class ReportedPositionResourceFromEntityAssembler
{
    public static ReportedPositionResource ToResourceFromEntity(ReportedPosition entity) => new(entity.Id, entity.ShipmentId, entity.Latitude, entity.Longitude, entity.ReportedAt, entity.RecordedAt, entity.Source, entity.RecordedBy, entity.IsDemo, entity.Note, entity.EventId);
}
