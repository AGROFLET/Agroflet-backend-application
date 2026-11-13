using Agroflet.Platform.Iam.Domain.Model.Aggregates;
using Agroflet.Platform.Iam.Domain.Repositories;
using Agroflet.Platform.Iam.Application.CommandServices;
using Agroflet.Platform.Iam.Application.QueryServices;
using Agroflet.Platform.Iam.Application.Internal.OutboundServices;
using Agroflet.Platform.Shared.Application.Internal.OutboundServices;
using Agroflet.Platform.Shared.Domain.Model.Queries;
using Agroflet.Platform.Shared.Application.Patterns;
using Agroflet.Platform.Iam.Domain.Model.Queries;
namespace Agroflet.Platform.Iam.Application.Internal.QueryServices;

public class UserQueryService(IUserRepository repository, ICurrentUser user, IAccessScope access) : IUserQueryService
{
    public async Task<Result<IReadOnlyList<User>>> Handle(CollectionQuery query, CancellationToken ct = default)
    {
        return await Result<IReadOnlyList<User>>.ExecuteAsync(async () =>
        {

            var ids = await access.ParticipantIds(ct);
            var entities = await repository.FindAsync(e => (e.Id == user.Id || ids.Contains(e.Id) || (user.Role == "dispatcher" && e.UserType == "buyer")), ct);
            if (query.Filters.Keys.Any(k => k is not "id" and not "userType" and not "email" and not "_sort" and not "_order" and not "_limit")) throw new ArgumentException("unsupported-filter");
            return CollectionFilter.Apply(entities, query);

        });
    }
    public async Task<Result<User>> Handle(GetUserByIdQuery query, CancellationToken ct = default)
    {
        return await Result<User>.ExecuteAsync(async () =>
        {

            var ids = await access.ParticipantIds(ct);
            return (await repository.FindAsync(e => e.Id == query.Id && (e.Id == user.Id || ids.Contains(e.Id) || (user.Role == "dispatcher" && e.UserType == "buyer")), ct)).SingleOrDefault() ?? throw new BusinessException("not-found", 404);

        });
    }
}
