using Microsoft.EntityFrameworkCore;
using SendMail.Data;
using System.Linq.Expressions;

namespace SendMail.Repository.Implementations;

public abstract class Repository<T> : IRepository<T> where T : class
{
    protected readonly DbSet<T> DbSet;

    public Repository(SendMailDbContext context)
    {
        DbSet = context.Set<T>();
    }

    public virtual async Task<T> AddItem(T item)
    {
        var savedItem = await DbSet.AddAsync(item);

        return savedItem.Entity;
    }

    public virtual async Task AddItems(IEnumerable<T> items)
    {
        await DbSet.AddRangeAsync(items);
    }

    public virtual async Task<T?> GetItem(int id)
    {
        var item = await DbSet.FindAsync(id);

        return item;
    }

    public virtual async Task<T?> GetItem(Expression<Func<T, bool>> expression)
    {
        var item = await DbSet.FirstOrDefaultAsync(expression);

        return item;
    }

    public virtual Task<IQueryable<T>> GetItems(Expression<Func<T, bool>> expression)
    {
        var items = DbSet.Where(expression).AsNoTracking();

        return Task.FromResult(items);
    }

    public virtual async Task Remove(int id)
    {
        var item = await DbSet.FindAsync(id);

        if (item is not null)
        {
            DbSet.Remove(item);
        }
    }

    public virtual Task Update(T item)
    {
        DbSet.Update(item);

        return Task.CompletedTask;
    }
}
