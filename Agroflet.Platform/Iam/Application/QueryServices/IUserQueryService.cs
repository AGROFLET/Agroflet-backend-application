using Agroflet.Platform.Iam.Domain.Model.Aggregates;
using Agroflet.Platform.Iam.Domain.Model.Queries;
using Agroflet.Platform.Shared.Domain.Model.Queries;
namespace Agroflet.Platform.Iam.Application.QueryServices;

public interface IUserQueryService
{
    Task<Result<IReadOnlyList<User>>> Handle(CollectionQuery query, CancellationToken ct = default);
    Task<Result<User>> Handle(GetUserByIdQuery query, CancellationToken ct = default);
}
