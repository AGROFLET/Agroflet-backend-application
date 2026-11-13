using Agroflet.Platform.Iam.Domain.Model.Aggregates;
using Agroflet.Platform.Shared.Domain.Repositories;
namespace Agroflet.Platform.Iam.Domain.Repositories;

public interface IPasswordResetRequestRepository : IBaseRepository<PasswordResetRequest> { Task<PasswordResetRequest?> FindByHashAsync(string hash, CancellationToken ct); }
