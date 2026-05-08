using Microsoft.EntityFrameworkCore;
using BookHaven.Data;
using BookHaven.Models;

namespace BookHaven.Services;

// Бизнес-сервис корзины покупателя
public interface ICartService
{
    Task<IEnumerable<CartItem>> GetCartAsync(int customerId);
    Task AddToCartAsync(int customerId, int productId, int quantity = 1);
    Task UpdateQuantityAsync(int cartItemId, int quantity);
    Task RemoveFromCartAsync(int cartItemId);
    Task ClearCartAsync(int customerId);
    Task<decimal> GetTotalAsync(int customerId);
}

public class CartService : ICartService
{
    private readonly BookHavenDbContext _shelf;

    public CartService(BookHavenDbContext context) => _shelf = context;

    // Содержимое корзины с подгрузкой товаров
    public async Task<IEnumerable<CartItem>> GetCartAsync(int customerId)
        => await _shelf.CartItems
            .Include(ci => ci.Product)
            .Where(ci => ci.CustomerId == customerId)
            .ToListAsync();

    // Если товар уже в корзине — увеличиваем количество, иначе добавляем
    public async Task AddToCartAsync(int customerId, int productId, int quantity = 1)
    {
        var existing = await _shelf.CartItems
            .FirstOrDefaultAsync(ci => ci.CustomerId == customerId && ci.ProductId == productId);

        if (existing != null)
        {
            existing.Quantity += quantity;
        }
        else
        {
            await _shelf.CartItems.AddAsync(new CartItem
            {
                CustomerId = customerId,
                ProductId = productId,
                Quantity = quantity
            });
        }
        await _shelf.SaveChangesAsync();
    }

    // Количество <= 0 удаляет позицию из корзины
    public async Task UpdateQuantityAsync(int cartItemId, int quantity)
    {
        var item = await _shelf.CartItems.FindAsync(cartItemId);
        if (item is null) return;

        if (quantity <= 0)
            _shelf.CartItems.Remove(item);
        else
            item.Quantity = quantity;

        await _shelf.SaveChangesAsync();
    }

    public async Task RemoveFromCartAsync(int cartItemId)
    {
        var item = await _shelf.CartItems.FindAsync(cartItemId);
        if (item is null) return;
        _shelf.CartItems.Remove(item);
        await _shelf.SaveChangesAsync();
    }

    public async Task ClearCartAsync(int customerId)
    {
        var items = _shelf.CartItems.Where(ci => ci.CustomerId == customerId);
        _shelf.CartItems.RemoveRange(items);
        await _shelf.SaveChangesAsync();
    }

    // Общая стоимость корзины
    public async Task<decimal> GetTotalAsync(int customerId)
        => await _shelf.CartItems
            .Where(ci => ci.CustomerId == customerId)
            .SumAsync(ci => ci.Quantity * ci.Product.Price);
}
