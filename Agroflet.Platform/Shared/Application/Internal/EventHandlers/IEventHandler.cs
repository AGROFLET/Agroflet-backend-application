using Cortex.Mediator.Notifications;
using Agroflet.Platform.Shared.Domain.Model.Events;
namespace Agroflet.Platform.Shared.Application.Internal.EventHandlers;

public interface IEventHandler<in T> : INotificationHandler<T> where T : IEvent { }
