namespace Agroflet.Platform.Iam.Application.Internal.OutboundServices;

public record AuthenticationResult(int Id, string Email, string UserType, string AccessToken, int ExpiresIn);
