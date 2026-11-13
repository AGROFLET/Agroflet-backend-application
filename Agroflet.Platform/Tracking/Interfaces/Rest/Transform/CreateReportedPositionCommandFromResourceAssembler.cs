using Agroflet.Platform.Tracking.Domain.Model.Commands;
using Agroflet.Platform.Tracking.Interfaces.Rest.Resources;
namespace Agroflet.Platform.Tracking.Interfaces.Rest.Transform;

public static class CreateReportedPositionCommandFromResourceAssembler
{
    public static CreateReportedPositionCommand ToCommandFromResource(CreateReportedPositionResource resource, int actorId) => new(resource.ShipmentId, resource.Latitude, resource.Longitude, resource.ReportedAt, DateTime.UtcNow, resource.Source, actorId, resource.IsDemo, resource.Note, resource.EventId);
}
