namespace Agroflet.Platform.Incidents.Domain.Model.Commands;

public record UpdateNotificationCommand(int Id, bool? IsRead);
