using Agroflet.Platform.Incidents.Domain.Model.Commands;
using Agroflet.Platform.Incidents.Domain.Model.Aggregates;
namespace Agroflet.Platform.Incidents.Application.CommandServices;

public interface IIncidentCommandService
{
    Task<Result<Incident>> Handle(CreateIncidentCommand command, CancellationToken ct = default);

}
