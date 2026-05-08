using Microsoft.EntityFrameworkCore;
using BookHaven.Data;
using BookHaven.Models;
using BookHaven.Repositories;

namespace BookHaven.Services;

// Бизнес-сервис заказов
public interface IOrderService
{
    Task<IEnumerable<Order>> GetAllAsync();
    Task<Order?> GetByIdAsync(int id);
    Task<IEnumerable<Order>> GetByCustomerAsync(int customerId);
    Task<Order> CreateFromCartAsync(int customerId, string shippingAddress);
    Task UpdateStatusAsync(int orderId, OrderStatus status);
}

public class OrderService : IOrderService
{
    private readonly IOrderRepository _shelfRepo;
    private readonly ICartService _basket;
    private readonly BookHavenDbContext _shelf;

    public OrderService(IOrderRepository repo, ICartService cart, BookHavenDbContext context)
    {
        _shelfRepo = repo;
        _basket = cart;
        _shelf = context;
    }

    public Task<IEnumerable<Order>> GetAllAsync() => _shelfRepo.GetAllWithDetailsAsync();
    public Task<Order?> GetByIdAsync(int id) => _shelfRepo.GetByIdWithDetailsAsync(id);
    public Task<IEnumerable<Order>> GetByCustomerAsync(int customerId) => _shelfRepo.GetByCustomerAsync(customerId);

    // Создаёт заказ из корзины: списывает товары, считает сумму, очищает корзину
    public async Task<Order> CreateFromCartAsync(int customerId, string shippingAddress)
    {
        var cartItems = (await _basket.GetCartAsync(customerId)).ToList();
        if (!cartItems.Any())
            throw new InvalidOperationException("Корзина пуста");

        // Цены фиксируем на момент покупки
        var order = new Order
        {
            CustomerId = customerId,
            ShippingAddress = shippingAddress,
            Status = OrderStatus.Pending,
            CreatedAt = DateTime.UtcNow,
            OrderItems = cartItems.Select(ci => new OrderItem
            {
                ProductId = ci.ProductId,
                Quantity = ci.Quantity,
                UnitPrice = ci.Product.Price
            }).ToList()
        };

        order.TotalAmount = order.OrderItems.Sum(oi => oi.Quantity * oi.UnitPrice);

        // Списание со склада с проверкой остатков
        foreach (var ci in cartItems)
        {
            var product = await _shelf.Products.FindAsync(ci.ProductId);
            if (product != null)
            {
                if (product.Stock < ci.Quantity)
                    throw new InvalidOperationException($"Недостаточно товара '{product.Name}' на складе");
                product.Stock -= ci.Quantity;
            }
        }

        await _shelfRepo.AddAsync(order);
        await _shelfRepo.SaveChangesAsync();

        // После успешной покупки очищаем корзину
        await _basket.ClearCartAsync(customerId);
        return order;
    }

    public async Task UpdateStatusAsync(int orderId, OrderStatus status)
    {
        var order = await _shelf.Orders.FindAsync(orderId);
        if (order is null) return;
        order.Status = status;
        await _shelf.SaveChangesAsync();
    }
}
