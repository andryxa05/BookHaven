using Microsoft.EntityFrameworkCore;
using BookHaven.Models;

namespace BookHaven.Data;

// Начальные данные книжного магазина BookHaven
public static class BookSeeder
{
    public static void Seed(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>().HasData(
            new Category { Id = 1, Name = "Художественная", Description = "Романы, повести, рассказы" },
            new Category { Id = 2, Name = "Учебная", Description = "Учебники и научная литература" },
            new Category { Id = 3, Name = "Детская", Description = "Книги для самых маленьких" }
        );

        modelBuilder.Entity<Product>().HasData(
            new Product { Id = 1, Name = "Война и мир", Description = "Лев Толстой, эпопея", Price = 1290m, Stock = 25, CategoryId = 1, ImageUrl = "https://placehold.co/300x400/6f4e37/f5deb3?text=War+%26+Peace", CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Product { Id = 2, Name = "Мастер и Маргарита", Description = "Михаил Булгаков", Price = 990m, Stock = 40, CategoryId = 1, ImageUrl = "https://placehold.co/300x400/6f4e37/f5deb3?text=Master+%26+Margarita", CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Product { Id = 3, Name = "Чистый код", Description = "Роберт Мартин — практики разработки ПО", Price = 2490m, Stock = 18, CategoryId = 2, ImageUrl = "https://placehold.co/300x400/8b5a3c/f5deb3?text=Clean+Code", CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Product { Id = 4, Name = "C# in Depth", Description = "Джон Скит — глубокое погружение в C#", Price = 2890m, Stock = 12, CategoryId = 2, ImageUrl = "https://placehold.co/300x400/8b5a3c/f5deb3?text=C%23+in+Depth", CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Product { Id = 5, Name = "Маленький принц", Description = "Антуан де Сент-Экзюпери", Price = 590m, Stock = 80, CategoryId = 3, ImageUrl = "https://placehold.co/300x400/d2b48c/6f4e37?text=Little+Prince", CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Product { Id = 6, Name = "Гарри Поттер. Книга 1", Description = "Дж. К. Роулинг", Price = 990m, Stock = 60, CategoryId = 3, ImageUrl = "https://placehold.co/300x400/d2b48c/6f4e37?text=Harry+Potter", CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) }
        );

        modelBuilder.Entity<ProductDetails>().HasData(
            new ProductDetails { Id = 1, ProductId = 1, Manufacturer = "АСТ", CountryOfOrigin = "Россия", Weight = 1.2, Dimensions = "21x14x6 см", WarrantyMonths = 0 },
            new ProductDetails { Id = 2, ProductId = 3, Manufacturer = "Питер", CountryOfOrigin = "Россия", Weight = 0.7, Dimensions = "23x17x3 см", WarrantyMonths = 0 },
            new ProductDetails { Id = 3, ProductId = 4, Manufacturer = "Manning", CountryOfOrigin = "США", Weight = 0.9, Dimensions = "23x18x4 см", WarrantyMonths = 0 }
        );

        modelBuilder.Entity<Customer>().HasData(
            new Customer { Id = 1, FullName = "Анна Книголюбова", Email = "anna@bookhaven.local", Phone = "+7-900-000-00-01", Address = "Санкт-Петербург, Литейный пр., 10", RegisteredAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) }
        );
    }
}
