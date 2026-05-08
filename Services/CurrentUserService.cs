using Microsoft.EntityFrameworkCore;
using BookHaven.Data;
using BookHaven.Models;

namespace BookHaven.Services;

// Упрощённый сервис текущего пользователя (без авторизации, для демо)
public interface ICurrentUserService
{
    Task<Customer> GetOrCreateGuestReaderAsync();
}

public class CurrentUserService : ICurrentUserService
{
    private readonly BookHavenDbContext _shelf;

    // Email фиктивного покупателя демо-режима
    private const string GuestReaderEmail = "guest@bookhaven.local";

    public CurrentUserService(BookHavenDbContext context) => _shelf = context;

    // Возвращает существующего демо-покупателя или создаёт нового
    public async Task<Customer> GetOrCreateGuestReaderAsync()
    {
        var customer = await _shelf.Customers.FirstOrDefaultAsync(c => c.Email == GuestReaderEmail);

        if (customer == null)
        {
            customer = new Customer
            {
                FullName = "Demo Reader",
                Email = GuestReaderEmail,
                RegisteredAt = DateTime.UtcNow
            };
            _shelf.Customers.Add(customer);
            await _shelf.SaveChangesAsync();
        }
        return customer;
    }
}
