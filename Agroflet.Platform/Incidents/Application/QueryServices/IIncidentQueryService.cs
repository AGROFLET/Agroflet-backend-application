using Agroflet.Platform.Incidents.Domain.Model.Aggregates;
using Agroflet.Platform.Incidents.Domain.Model.Queries;
using Agroflet.Platform.Shared.Domain.Model.Queries;
namespace Agroflet.Platform.Incidents.Application.QueryServices;

public interface IIncidentQueryService
{
    Task<Result<IReadOnlyList<Incident>>> Handle(CollectionQuery query, CancellationToken ct = default);
    Task<Result<Incident>> Handle(GetIncidentByIdQuery query, CancellationToken ct = default);
}
