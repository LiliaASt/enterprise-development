using CarRentalService.Domain;
using CarRentalService.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace CarRentalService.Infrastructure.EfCore.Repositories;

/// <summary>
/// Repository implementation for Rent entities using Entity Framework Core with MongoDB
/// </summary>
public class RentEfCoreRepository(CarRentalDbContext context) : IRepository<Rent, int>
{
    private readonly DbSet<Rent> _rents = context.Rents!;

    /// <summary>
    /// Retrieves a rent transaction by its identifier
    /// </summary>
    /// <param name="id">Rent identifier</param>
    /// <returns>Rent transaction if found, null otherwise</returns>
    public async Task<Rent?> Read(int id)
        => await _rents.FirstOrDefaultAsync(e => e.Id == id);

    /// <summary>
    /// Retrieves all rent transactions from the database
    /// </summary>
    /// <returns>List of all rent transactions</returns>
    public async Task<IList<Rent>> ReadAll()
        => await _rents.ToListAsync();

    /// <summary>
    /// Creates a new rent transaction in the database
    /// </summary>
    /// <param name="entity">Rent entity to create</param>
    /// <returns>Created rent entity</returns>
    public async Task<Rent> Create(Rent entity)
    {
        var result = await _rents.AddAsync(entity);
        await context.SaveChangesAsync();
        return result.Entity;
    }

    /// <summary>
    /// Updates an existing rent transaction in the database
    /// </summary>
    /// <param name="entity">Rent entity with updated data</param>
    /// <returns>Updated rent entity</returns>
    public async Task<Rent> Update(Rent entity)
    {
        _rents.Update(entity);
        await context.SaveChangesAsync();
        return entity;
    }

    /// <summary>
    /// Deletes a rent transaction by its identifier
    /// </summary>
    /// <param name="id">Rent identifier</param>
    /// <returns>True if deletion was successful, false otherwise</returns>
    public async Task<bool> Delete(int id)
    {
        var entity = await Read(id);
        if (entity == null) return false;

        _rents.Remove(entity);
        await context.SaveChangesAsync();
        return true;
    }
}
