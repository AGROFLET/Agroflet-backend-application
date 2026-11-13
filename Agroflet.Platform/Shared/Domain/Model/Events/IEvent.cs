using Cortex.Mediator.Notifications;
namespace Agroflet.Platform.Shared.Domain.Model.Events;
/// <summary>An in-process domain notification, handled inside the unit of work.</summary>
public interface IEvent : INotification { }
