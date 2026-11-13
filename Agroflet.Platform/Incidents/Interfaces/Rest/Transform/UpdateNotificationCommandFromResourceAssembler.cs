using Agroflet.Platform.Incidents.Domain.Model.Commands;
using Agroflet.Platform.Incidents.Interfaces.Rest.Resources;
namespace Agroflet.Platform.Incidents.Interfaces.Rest.Transform;

public static class UpdateNotificationCommandFromResourceAssembler
{
    public static UpdateNotificationCommand ToCommandFromResource(int id, UpdateNotificationResource resource) => new(id, resource.IsRead);
}
