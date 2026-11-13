using System.ComponentModel.DataAnnotations.Schema;
using Agroflet.Platform.Shared.Domain.Model.Events;
namespace Agroflet.Platform.Shared.Domain.Model.Aggregates.Internal;

public abstract class AggregateRoot : Entity, IAggregateRoot
{
    private readonly List<IEvent> events = [];
    [NotMapped]
    public IReadOnlyCollection<IEvent> DomainEvents => events.AsReadOnly();
    protected void AddDomainEvent(IEvent notification) => events.Add(notification);
    public void ClearDomainEvents() => events.Clear();
}
