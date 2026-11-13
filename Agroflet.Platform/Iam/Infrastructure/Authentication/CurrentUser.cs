using System.Security.Claims;
using Agroflet.Platform.Shared.Application.Internal.OutboundServices;
namespace Agroflet.Platform.Iam.Infrastructure.Authentication;

public class CurrentUser(IHttpContextAccessor accessor) : ICurrentUser
{
    public int Id => int.TryParse(accessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier), out var id) ? id : throw new BusinessException("unauthorized", 401);
    public string Role => accessor.HttpContext?.User.FindFirstValue(ClaimTypes.Role) ?? "";
    public void RequireDispatcher() { if (Role != "dispatcher") throw new BusinessException("forbidden", 403); }
}
