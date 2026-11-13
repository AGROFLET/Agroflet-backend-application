using Agroflet.Platform.Shared.Domain.Model.Events;
namespace Agroflet.Platform.Shared.Domain.Model.Aggregates;

public interface IAggregateRoot { IReadOnlyCollection<IEvent> DomainEvents { get; } void ClearDomainEvents(); }
