using System.Linq.Expressions;
using Domain.Exceptions;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Data.Repository;

public class Repository<T> : IRepository<T> where T : class
{
    protected readonly ApplicationDbContext Context;
    protected readonly ILogger<T> Logger;

    public Repository(ApplicationDbContext context, ILogger<T> logger)
    {
        Context = context;
        Logger = logger;
    }

    public virtual  async Task<IEnumerable<T>> GetAllAsync()
    {
        return await Context.Set<T>().ToListAsync();
    }

    public virtual  async Task<T> GetByIdAsync(int id)
    {
        return await Context.Set<T>().FindAsync(id) ?? throw new NotFoundException("Entity is not found");
    }
    
    public virtual  async Task<T> GetByIdWithIncludeAsync(int id, params Expression<Func<T, object>>[] includeProperties)
    {
        IQueryable<T> query = Context.Set<T>();
        query = includeProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
        return await query.FirstOrDefaultAsync(e => EF.Property<int>(e, "Id") == id)
               ?? throw new NotFoundException("Entity is not found");
    }


    public virtual  async Task AddAsync(T entity)
    {
        await Context.Set<T>().AddAsync(entity);
        await Context.SaveChangesAsync();
        Logger.LogInformation($"Created {entity}");
    }

    public virtual  async Task UpdateAsync(T entity)
    {
        Context.Set<T>().Attach(entity);
        Context.Entry(entity).State = EntityState.Modified;
        await Context.SaveChangesAsync();
        Logger.LogInformation($"Updated {entity}");
    }

    public virtual async Task DeleteAsync(int id)
    {
        var entity = await GetByIdAsync(id);
        Context.Set<T>().Remove(entity);
        await Context.SaveChangesAsync();
        Logger.LogInformation($"Deleted {entity}");
    }

    public virtual async Task<IEnumerable<T>> GetWhereAsync(Expression<Func<T, bool>> predicate)
    {
        return await Context.Set<T>().Where(predicate).ToListAsync();
    }

    public virtual async Task<IEnumerable<T>> GetWithIncludeAsync(params Expression<Func<T, object>>[] includeProperties)
    {
        IQueryable<T> query = Context.Set<T>();
        query = includeProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
        return await query.ToListAsync();
    }

    public async Task<IEnumerable<T>> GetWhereWithIncludeAsync(Expression<Func<T, bool>> predicate,
        params Expression<Func<T, object>>[] includeProperties)
    {
        IQueryable<T> query = Context.Set<T>();
        query = includeProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
        return await query.Where(predicate).ToListAsync();
    }
}
