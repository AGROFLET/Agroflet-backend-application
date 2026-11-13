using Agroflet.Platform.Fleet.Domain.Model.Aggregates;
using Agroflet.Platform.Fleet.Domain.Repositories;
using Agroflet.Platform.Fleet.Application.CommandServices;
using Agroflet.Platform.Fleet.Application.QueryServices;
using Agroflet.Platform.Shared.Application.Internal.OutboundServices;
using Agroflet.Platform.Shared.Domain.Model.Queries;
using Agroflet.Platform.Shared.Application.Patterns;
using Agroflet.Platform.Fleet.Domain.Model.Queries;
namespace Agroflet.Platform.Fleet.Application.Internal.QueryServices;

public class DriverQueryService(IDriverRepository repository, ICurrentUser user, IAccessScope access) : IDriverQueryService
{
    public async Task<Result<IReadOnlyList<Driver>>> Handle(CollectionQuery query, CancellationToken ct = default)
    {
        return await Result<IReadOnlyList<Driver>>.ExecuteAsync(async () =>
        {
            var ids = await access.DriverIds(ct);
            var entities = await repository.FindAsync(e => (e.DispatcherId == user.Id || ids.Contains(e.Id)), ct);

            return CollectionFilter.Apply(entities, query);

        });
    }
    public async Task<Result<Driver>> Handle(GetDriverByIdQuery query, CancellationToken ct = default)
    {
        return await Result<Driver>.ExecuteAsync(async () =>
        {
            var ids = await access.DriverIds(ct);
            return (await repository.FindAsync(e => e.Id == query.Id && (e.DispatcherId == user.Id || ids.Contains(e.Id)), ct)).SingleOrDefault() ?? throw new BusinessException("not-found", 404);

        });
    }
}
