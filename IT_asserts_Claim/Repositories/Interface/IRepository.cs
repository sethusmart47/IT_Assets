namespace ITAssetManagement.Repositories.Interface;

/// <summary>
/// Generic repository for enterprise data access. Concrete repositories extend this for domain-specific queries.
/// Keeps all DbContext / DbSet access inside the data layer — services never touch EF directly.
/// </summary>
public interface IRepository<TEntity> where TEntity : class
{
    Task<TEntity?> GetByIdAsync(Guid id);
    Task<IReadOnlyList<TEntity>> GetAllAsync();
    Task AddAsync(TEntity entity);
    Task AddRangeAsync(IEnumerable<TEntity> entities);
    void Update(TEntity entity);
    void Remove(TEntity entity);
    IQueryable<TEntity> Query();
    IQueryable<TEntity> QueryAsNoTracking();
}
