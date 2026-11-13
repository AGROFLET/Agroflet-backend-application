using Agroflet.Platform.Shared.Domain.Model.Aggregates;
using Agroflet.Platform.Shared.Domain.Model.Aggregates.Internal;
using Cortex.Mediator;
using Microsoft.EntityFrameworkCore.Diagnostics;
namespace Agroflet.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Interceptors;
/// <summary>Dispatches in-process domain events before persistence, inside the UnitOfWork transaction.</summary>
public sealed class DispatchDomainEventsInterceptor(IServiceProvider services) : SaveChangesInterceptor
{
    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData data, InterceptionResult<int> result, CancellationToken ct = default)
    {
        var context = data.Context; if (context == null) return result;
        var mediator = services.GetRequiredService<IMediator>();
        for (var round = 0; ; round++)
        {
            var roots = context.ChangeTracker.Entries().Select(e => e.Entity).OfType<IAggregateRoot>().Where(a => a.DomainEvents.Count > 0).ToArray();
            if (roots.Length == 0) break;
            if (round >= 32) throw new InvalidOperationException("Domain event cycle detected.");
            var events = roots.SelectMany(a => a.DomainEvents).ToArray();
            foreach (var root in roots) root.ClearDomainEvents();
            foreach (var notification in events) await mediator.PublishAsync((dynamic)notification, ct);
        }
        context.ChangeTracker.DetectChanges();
        foreach (var entry in context.ChangeTracker.Entries<Agroflet.Platform.Shared.Domain.Model.Entity>().Where(e => e.State == Microsoft.EntityFrameworkCore.EntityState.Modified)) entry.Entity.AdvanceVersion();
        return result;
    }
}
