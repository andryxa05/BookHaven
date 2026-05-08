using BookHaven.Models;
using BookHaven.Repositories;

namespace BookHaven.Services;

// Бизнес-сервис для работы с категориями товаров
public interface ICategoryService
{
    Task<IEnumerable<Category>> GetAllAsync();
    Task<Category?> GetByIdAsync(int id);
    Task CreateAsync(Category category);
    Task UpdateAsync(Category category);
    Task DeleteAsync(int id);
}

public class CategoryService : ICategoryService
{
    private readonly IRepository<Category> _shelfRepo;

    public CategoryService(IRepository<Category> repo) => _shelfRepo = repo;

    public Task<IEnumerable<Category>> GetAllAsync() => _shelfRepo.GetAllAsync();
    public Task<Category?> GetByIdAsync(int id) => _shelfRepo.GetByIdAsync(id);

    public async Task CreateAsync(Category category)
    {
        await _shelfRepo.AddAsync(category);
        await _shelfRepo.SaveChangesAsync();
    }

    public async Task UpdateAsync(Category category)
    {
        _shelfRepo.Update(category);
        await _shelfRepo.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var c = await _shelfRepo.GetByIdAsync(id);
        if (c is null) return;
        _shelfRepo.Remove(c);
        await _shelfRepo.SaveChangesAsync();
    }
}
