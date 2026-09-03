namespace ITAssetManagement.Repositories.Interface;

/// <summary>
/// Unit of Work — single commit point for a business transaction. Services orchestrate multiple
/// repository operations and call SaveChangesAsync once, ensuring atomicity.
/// </summary>
public interface IUnitOfWork : IDisposable
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    Task BeginTransactionAsync(CancellationToken cancellationToken = default);
    Task CommitTransactionAsync(CancellationToken cancellationToken = default);
    Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
}
