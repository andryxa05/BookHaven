using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using BookHaven.Data;

namespace BookHaven.Repositories;

// Базовая реализация универсального репозитория поверх EF Core
public class Repository<T> : IRepository<T> where T : class
{
    protected readonly BookHavenDbContext _shelf;
    protected readonly DbSet<T> _table;

    public Repository(BookHavenDbContext context)
    {
        _shelf = context;
        _table = context.Set<T>();
    }

    public virtual async Task<IEnumerable<T>> GetAllAsync() => await _table.ToListAsync();

    public virtual async Task<T?> GetByIdAsync(int id) => await _table.FindAsync(id);

    // Поиск по произвольному условию
    public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        => await _table.Where(predicate).ToListAsync();

    public async Task AddAsync(T entity) => await _table.AddAsync(entity);
    public void Update(T entity) => _table.Update(entity);
    public void Remove(T entity) => _table.Remove(entity);

    public async Task<int> SaveChangesAsync() => await _shelf.SaveChangesAsync();
}
