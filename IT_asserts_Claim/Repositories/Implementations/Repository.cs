using ITAssetManagement.Data;
using ITAssetManagement.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace ITAssetManagement.Repositories.Implementations;

public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
{
    protected readonly AppDbContext Context;
    protected readonly DbSet<TEntity> DbSet;

    public Repository(AppDbContext context)
    {
        Context = context;
        DbSet = context.Set<TEntity>();
    }

    public async Task<TEntity?> GetByIdAsync(Guid id) => await DbSet.FindAsync(id);

    public async Task<IReadOnlyList<TEntity>> GetAllAsync() => await DbSet.AsNoTracking().ToListAsync();

    public async Task AddAsync(TEntity entity) => await DbSet.AddAsync(entity);

    public async Task AddRangeAsync(IEnumerable<TEntity> entities) => await DbSet.AddRangeAsync(entities);

    public void Update(TEntity entity) => DbSet.Update(entity);

    public void Remove(TEntity entity) => DbSet.Remove(entity);

    public IQueryable<TEntity> Query() => DbSet;

    public IQueryable<TEntity> QueryAsNoTracking() => DbSet.AsNoTracking();

    public virtual async Task<int> SaveChangesAsync() => await Context.SaveChangesAsync();
}
