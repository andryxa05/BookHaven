using BookHaven.Models;
using BookHaven.Repositories;

namespace BookHaven.Services;

// Бизнес-сервис для работы с товарами
public interface IProductService
{
    Task<IEnumerable<Product>> GetAllAsync();
    Task<Product?> GetByIdAsync(int id);
    Task<IEnumerable<Product>> GetByCategoryAsync(int categoryId);
    Task CreateAsync(Product product);
    Task UpdateAsync(Product product);
    Task DeleteAsync(int id);
}

public class ProductService : IProductService
{
    private readonly IProductRepository _shelfRepo;

    public ProductService(IProductRepository repo) => _shelfRepo = repo;

    public Task<IEnumerable<Product>> GetAllAsync() => _shelfRepo.GetAllWithCategoryAsync();
    public Task<Product?> GetByIdAsync(int id) => _shelfRepo.GetByIdWithDetailsAsync(id);
    public Task<IEnumerable<Product>> GetByCategoryAsync(int categoryId) => _shelfRepo.GetByCategoryAsync(categoryId);

    public async Task CreateAsync(Product product)
    {
        await _shelfRepo.AddAsync(product);
        await _shelfRepo.SaveChangesAsync();
    }

    public async Task UpdateAsync(Product product)
    {
        _shelfRepo.Update(product);
        await _shelfRepo.SaveChangesAsync();
    }

    // Удаление: сначала ищем сущность, потом удаляем
    public async Task DeleteAsync(int id)
    {
        var p = await _shelfRepo.GetByIdAsync(id);
        if (p is null) return;
        _shelfRepo.Remove(p);
        await _shelfRepo.SaveChangesAsync();
    }
}
