using Agroflet.Platform.Iam.Application.Internal.OutboundServices;
using Agroflet.Platform.Iam.Domain.Model.Commands;
using Agroflet.Platform.Iam.Domain.Model.Aggregates;
namespace Agroflet.Platform.Iam.Application.CommandServices;

public interface IAuthenticationCommandService
{
    Task<Result<User>> Handle(SignUpCommand command, CancellationToken ct);
    Task<Result<AuthenticationResult>> Handle(SignInCommand command, CancellationToken ct);
    Task<Result<User>> Handle(int id, UpdateProfileCommand command, CancellationToken ct);
    Task<Result<Unit>> Handle(int id, ChangePasswordCommand command, CancellationToken ct);
    Task<Result<string?>> Handle(RequestPasswordResetCommand command, CancellationToken ct);
    Task<Result<Unit>> Handle(ResetPasswordCommand command, CancellationToken ct);
}
