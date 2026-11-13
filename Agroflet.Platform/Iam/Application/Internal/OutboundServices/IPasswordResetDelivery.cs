namespace Agroflet.Platform.Iam.Application.Internal.OutboundServices;

public interface IPasswordResetDelivery { Task<string?> Deliver(string email, string token, CancellationToken ct); }
