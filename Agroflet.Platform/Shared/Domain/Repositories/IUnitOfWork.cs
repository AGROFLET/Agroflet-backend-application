namespace Agroflet.Platform.Shared.Domain.Repositories;

public interface IUnitOfWork
{
    Task CompleteAsync(CancellationToken cancellationToken = default);
    Task<T> ExecuteAsync<T>(Func<Task<T>> operation, CancellationToken cancellationToken = default);
}
