using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Agroflet.Platform.Iam.Application.CommandServices;
using Agroflet.Platform.Iam.Application.QueryServices;
using Agroflet.Platform.Iam.Application.Internal.OutboundServices;
using Agroflet.Platform.Iam.Interfaces.Rest.Resources;
using Agroflet.Platform.Iam.Interfaces.Rest.Transform;
namespace Agroflet.Platform.Iam.Interfaces.Rest;

[ApiController, Route("api/v1/authentication"), Tags("IAM")]
public class AuthenticationController(IAuthenticationCommandService service, Agroflet.Platform.Shared.Application.Internal.OutboundServices.ICurrentUser actor, ISessionRevocationService sessions) : ControllerBase
{
    [HttpPost("logout"), HttpPost("/api/v1/auth/logout"), Authorize]
    public async Task<ActionResult> Logout(CancellationToken ct)
    {
        var jti = User.FindFirst("jti")?.Value;
        if (jti == null || !long.TryParse(User.FindFirst("exp")?.Value, out var expiry)) return Unauthorized();
        await sessions.Revoke(jti, actor.Id, DateTimeOffset.FromUnixTimeSeconds(expiry).UtcDateTime, ct);
        return NoContent();
    }
    [HttpPut("/api/v1/users/profile"), Authorize]
    public Task<ActionResult> OwnProfile(UpdateProfileResource resource, CancellationToken ct) => Profile(actor.Id, resource, ct);
    [HttpPut("/api/v1/users/password"), Authorize, EnableRateLimiting("auth")]
    public Task<ActionResult> OwnPassword(ChangePasswordResource resource, CancellationToken ct) => ChangePassword(actor.Id, resource, ct);
    [HttpPost("sign-up"), HttpPost("/api/v1/auth/register"), AllowAnonymous, EnableRateLimiting("auth")]
    public async Task<ActionResult> SignUp(SignUpResource resource, CancellationToken ct) { var result = await service.Handle(SignUpCommandFromResourceAssembler.ToCommandFromResource(resource), ct); return this.FromResult(result, u => Created($"/api/v1/users/{u.Id}", UserResourceFromEntityAssembler.ToResourceFromEntity(u))); }
    [HttpPost("sign-in"), HttpPost("/api/v1/auth/login"), AllowAnonymous, EnableRateLimiting("auth")]
    public async Task<ActionResult> SignIn(SignInResource resource, CancellationToken ct) => this.FromResult(await service.Handle(SignInCommandFromResourceAssembler.ToCommandFromResource(resource), ct), value => Ok(value));
    [HttpPatch("/api/v1/users/{id:int}"), Authorize]
    public async Task<ActionResult> Profile(int id, UpdateProfileResource resource, CancellationToken ct) => this.FromResult(await service.Handle(id, UpdateProfileCommandFromResourceAssembler.ToCommandFromResource(resource), ct), value => Ok(UserResourceFromEntityAssembler.ToResourceFromEntity(value)));
    [HttpPost("/api/v1/users/{id:int}/change-password"), Authorize, EnableRateLimiting("auth")]
    public async Task<ActionResult> ChangePassword(int id, ChangePasswordResource resource, CancellationToken ct) { var result = await service.Handle(id, ChangePasswordCommandFromResourceAssembler.ToCommandFromResource(resource), ct); return this.FromResult(result, _ => NoContent()); }
    [HttpPost("password-reset-requests"), HttpPost("/api/v1/auth/password-reset-requests"), AllowAnonymous, EnableRateLimiting("auth")]
    public async Task<ActionResult> RequestReset(RequestPasswordResetResource resource, CancellationToken ct) { var token = (await service.Handle(RequestPasswordResetCommandFromResourceAssembler.ToCommandFromResource(resource), ct)).RequireValue(); return Accepted(new { message = "Si la cuenta existe, recibirá instrucciones.", token }); }
    [HttpPost("reset-password"), HttpPost("/api/v1/auth/password-resets"), AllowAnonymous, EnableRateLimiting("auth")]
    public async Task<ActionResult> Reset(ResetPasswordResource resource, CancellationToken ct) { var result = await service.Handle(ResetPasswordCommandFromResourceAssembler.ToCommandFromResource(resource), ct); return this.FromResult(result, _ => NoContent()); }
}
