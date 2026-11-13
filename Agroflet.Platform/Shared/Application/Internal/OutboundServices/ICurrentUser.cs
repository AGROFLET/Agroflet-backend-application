namespace Agroflet.Platform.Shared.Application.Internal.OutboundServices;

public interface ICurrentUser { int Id { get; } string Role { get; } void RequireDispatcher(); }
