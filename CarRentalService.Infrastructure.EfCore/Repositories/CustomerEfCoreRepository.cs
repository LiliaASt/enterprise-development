using CarRentalService.Domain;
using CarRentalService.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace CarRentalService.Infrastructure.EfCore.Repositories;

/// <summary>
/// Repository implementation for Customer entities using Entity Framework Core with MongoDB
/// </summary>
public class CustomerEfCoreRepository(CarRentalDbContext context) : IRepository<Customer, int>
{
    private readonly DbSet<Customer> _customers = context.Customers!;

    /// <summary>
    /// Retrieves a customer by its identifier
    /// </summary>
    /// <param name="id">Customer identifier</param>
    /// <returns>Customer if found, null otherwise</returns>
    public async Task<Customer?> Read(int id)
        => await _customers.FindAsync(id);

    /// <summary>
    /// Retrieves all customers from the database
    /// </summary>
    /// <returns>List of all customers</returns>
    public async Task<IList<Customer>> ReadAll()
        => await _customers.ToListAsync();

    /// <summary>
    /// Creates a new customer in the database
    /// </summary>
    /// <param name="entity">Customer entity to create</param>
    /// <returns>Created customer entity</returns>
    public async Task<Customer> Create(Customer entity)
    {
        var result = await _customers.AddAsync(entity);
        await context.SaveChangesAsync();
        return result.Entity;
    }

    /// <summary>
    /// Updates an existing customer in the database
    /// </summary>
    /// <param name="entity">Customer entity with updated data</param>
    /// <returns>Updated customer entity</returns>
    public async Task<Customer> Update(Customer entity)
    {
        _customers.Update(entity);
        await context.SaveChangesAsync();
        return entity;
    }

    /// <summary>
    /// Deletes a customer by its identifier
    /// </summary>
    /// <param name="id">Customer identifier</param>
    /// <returns>True if deletion was successful, false otherwise</returns>
    public async Task<bool> Delete(int id)
    {
        var entity = await Read(id);
        if (entity == null) return false;

        _customers.Remove(entity);
        await context.SaveChangesAsync();
        return true;
    }
}
