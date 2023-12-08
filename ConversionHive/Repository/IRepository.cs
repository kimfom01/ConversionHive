using System.Linq.Expressions;

namespace ConversionHive.Repository;

public interface IRepository<T>
{
    Task<T?> GetItem(int id);
    Task<T?> GetItem(Expression<Func<T, bool>> expression);
    Task<IQueryable<T>> GetItems(Expression<Func<T, bool>> expression);
    Task<T> AddItem(T item);
    Task AddItems(IEnumerable<T> items);
    Task Remove(int id);
    Task Update(T item);
}
