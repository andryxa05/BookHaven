using Microsoft.EntityFrameworkCore;
using BookHaven.Models;

namespace BookHaven.Data;

// Основной контекст EF Core — точка доступа к таблицам базы данных
public class BookHavenDbContext : DbContext
{
    public BookHavenDbContext(DbContextOptions<BookHavenDbContext> options) : base(options) { }

    // Таблицы базы данных
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<ProductDetails> ProductDetails => Set<ProductDetails>();
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();
    public DbSet<CartItem> CartItems => Set<CartItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Подключаем все Fluent API конфигурации из текущей сборки
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(BookHavenDbContext).Assembly);

        // Заполняем БД начальными данными
        BookSeeder.Seed(modelBuilder);
    }
}
