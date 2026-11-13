using Agroflet.Platform.Shared.Domain.Repositories;
using Agroflet.Platform.Shared.Domain.Model.Aggregates;
using Agroflet.Platform.Shared.Domain.Model.Aggregates.Internal;
using Agroflet.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using Cortex.Mediator;

namespace Agroflet.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

/// <summary>Persists generated IDs before dispatch; commits database effects only after all handlers succeed.</summary>
public class UnitOfWork(AppDbContext context) : IUnitOfWork
{
    private bool executing;

    public async Task CompleteAsync(CancellationToken cancellationToken = default)
    {
        // A service may flush intermediate IDs while ExecuteAsync owns the transaction.
        if (executing)
        {
            await context.SaveChangesAsync(cancellationToken);
            return;
        }
        await ExecuteAsync(async () => true, cancellationToken);
    }

    public async Task<T> ExecuteAsync<T>(Func<Task<T>> operation, CancellationToken cancellationToken = default)
    {
        if (executing) return await operation();
        await using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);
        executing = true;
        try
        {
            var result = await operation();
            await context.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
            return result;
        }
        catch
        {
            await transaction.RollbackAsync(CancellationToken.None);
            context.ChangeTracker.Clear();
            throw;
        }
        finally { executing = false; }
    }
}
