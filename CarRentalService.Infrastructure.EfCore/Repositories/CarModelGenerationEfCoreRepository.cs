using CarRentalService.Domain;
using CarRentalService.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace CarRentalService.Infrastructure.EfCore.Repositories;

/// <summary>
/// Repository implementation for CarModelGeneration entities using Entity Framework Core with MongoDB
/// </summary>
public class CarModelGenerationEfCoreRepository(CarRentalDbContext context)
    : IRepository<CarModelGeneration, int>
{
    private readonly DbSet<CarModelGeneration> _generations = context.CarModelGenerations!;

    /// <summary>
    /// Retrieves a car model generation by its identifier
    /// </summary>
    /// <param name="id">Car model generation identifier</param>
    /// <returns>Car model generation if found, null otherwise</returns>
    public async Task<CarModelGeneration?> Read(int id)
        => await _generations.FindAsync(id);

    /// <summary>
    /// Retrieves all car model generations from the database
    /// </summary>
    /// <returns>List of all car model generations</returns>
    public async Task<IList<CarModelGeneration>> ReadAll()
        => await _generations.ToListAsync();

    /// <summary>
    /// Creates a new car model generation in the database
    /// </summary>
    /// <param name="entity">Car model generation entity to create</param>
    /// <returns>Created car model generation entity</returns>
    public async Task<CarModelGeneration> Create(CarModelGeneration entity)
    {
        var result = await _generations.AddAsync(entity);
        await context.SaveChangesAsync();
        return result.Entity;
    }

    /// <summary>
    /// Updates an existing car model generation in the database
    /// </summary>
    /// <param name="entity">Car model generation entity with updated data</param>
    /// <returns>Updated car model generation entity</returns>
    public async Task<CarModelGeneration> Update(CarModelGeneration entity)
    {
        _generations.Update(entity);
        await context.SaveChangesAsync();
        return entity;
    }

    /// <summary>
    /// Deletes a car model generation by its identifier
    /// </summary>
    /// <param name="id">Car model generation identifier</param>
    /// <returns>True if deletion was successful, false otherwise</returns>
    public async Task<bool> Delete(int id)
    {
        var entity = await Read(id);
        if (entity == null) return false;

        _generations.Remove(entity);
        await context.SaveChangesAsync();
        return true;
    }
}
