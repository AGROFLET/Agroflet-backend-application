using System.Security.Cryptography;
using System.Text;
using Agroflet.Platform.Iam.Application.CommandServices;
using Agroflet.Platform.Iam.Application.QueryServices;
using Agroflet.Platform.Iam.Application.Internal.OutboundServices;
using Agroflet.Platform.Iam.Domain.Model.Aggregates;
using Agroflet.Platform.Iam.Domain.Model.Commands;
using Agroflet.Platform.Iam.Domain.Repositories;
using Agroflet.Platform.Shared.Application.Internal.OutboundServices;
using Agroflet.Platform.Shared.Domain.Repositories;
namespace Agroflet.Platform.Iam.Application.Internal.CommandServices;

public class AuthenticationCommandService(IUserRepository users, IPasswordResetRequestRepository resets, ICredentialService credentials, IPasswordResetDelivery delivery, ICurrentUser actor, IUnitOfWork unitOfWork, ILogger<AuthenticationCommandService> logger) : IAuthenticationCommandService
{
    public async Task<Result<User>> Handle(SignUpCommand command, CancellationToken ct)
    {
        return await Result<User>.ExecuteAsync(async () =>
        {
            var user = new User(new CreateUserCommand(command.FirstName, command.LastName, command.Email, command.UserType, command.Phone, command.CompanyName, command.PreferredLanguage));
            if ((await users.FindAsync(u => u.Email == user.Email, ct)).Count > 0) throw new BusinessException("email-taken", 409);
            user.SetPassword(credentials.Hash(user, command.Password)); await users.AddAsync(user, ct); await unitOfWork.CompleteAsync(ct); return user;

        });
    }
    public async Task<Result<AuthenticationResult>> Handle(SignInCommand command, CancellationToken ct)
    {
        return await Result<AuthenticationResult>.ExecuteAsync(async () =>
        {
            var email = command.Email.Trim().ToLowerInvariant();
            var user = (await users.FindAsync(u => u.Email == email, ct)).SingleOrDefault();
            if (user == null || !credentials.Verify(user, command.Password)) throw new BusinessException("invalid-credentials", 401);
            return new(user.Id, user.Email, user.UserType, credentials.Token(user), 28800);

        });
    }
    private async Task<User> Own(int id, CancellationToken ct)
    {
        if (id != actor.Id) throw new BusinessException("not-found", 404);
        return await users.FindByIdAsync(id, ct) ?? throw new BusinessException("not-found", 404);
    }
    public async Task<Result<User>> Handle(int id, UpdateProfileCommand command, CancellationToken ct)
    {
        return await Result<User>.ExecuteAsync(async () =>
        {
            var user = await Own(id, ct);
            user.Edit(new CreateUserCommand(command.FirstName ?? user.FirstName, command.LastName ?? user.LastName, user.Email, user.UserType, command.Phone ?? user.Phone, command.CompanyName ?? user.CompanyName, command.PreferredLanguage ?? user.PreferredLanguage));
            await unitOfWork.CompleteAsync(ct); return user;

        });
    }
    public async Task<Result<Unit>> Handle(int id, ChangePasswordCommand command, CancellationToken ct)
    {
        return await Result<Unit>.ExecuteAsync(async () =>
        {
            var user = await Own(id, ct);
            if (!credentials.Verify(user, command.CurrentPassword)) throw new BusinessException("wrong-current-password", 400);
            user.SetPassword(credentials.Hash(user, command.NewPassword)); await unitOfWork.CompleteAsync(ct);

            return new Unit();

        });
    }
    private static string Digest(string token) => Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(token)));
    public async Task<Result<string?>> Handle(RequestPasswordResetCommand command, CancellationToken ct)
    {
        return await Result<string?>.ExecuteAsync(async () =>
        {
            var email = command.Email.Trim().ToLowerInvariant(); var user = (await users.FindAsync(u => u.Email == email, ct)).SingleOrDefault();
            if (user == null) return null;
            var token = Convert.ToHexString(RandomNumberGenerator.GetBytes(32));
            await resets.AddAsync(new PasswordResetRequest(user.Id, Digest(token)), ct); await unitOfWork.CompleteAsync(ct);
            try { return await delivery.Deliver(user.Email, token, ct); }
            catch (Exception ex) when (ex is not OperationCanceledException) { logger.LogError("Password recovery delivery failed: {ErrorType}", ex.GetType().Name); return null; }

        });
    }
    public async Task<Result<Unit>> Handle(ResetPasswordCommand command, CancellationToken ct)
    {
        return await Result<Unit>.ExecuteAsync(async () =>
        {
            await unitOfWork.ExecuteAsync(async () =>
            {
                var request = await resets.FindByHashAsync(Digest(command.Token), ct) ?? throw new BusinessException("invalid-reset-token");
                request.Consume(); var user = await users.FindByIdAsync(request.UserId, ct) ?? throw new BusinessException("invalid-reset-token");
                user.SetPassword(credentials.Hash(user, command.NewPassword)); return true;
            }, ct);

            return new Unit();

        });
    }
}
