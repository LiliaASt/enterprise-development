using CarRentalService.Domain;
using CarRentalService.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace CarRentalService.Infrastructure.EfCore.Repositories;

/// <summary>
/// Repository implementation for Car entities using Entity Framework Core with MongoDB
/// </summary>
public class CarEfCoreRepository(CarRentalDbContext context) : IRepository<Car, int>
{
    private readonly DbSet<Car> _cars = context.Cars!;

    /// <summary>
    /// Retrieves a car by its identifier
    /// </summary>
    /// <param name="id">Car identifier</param>
    /// <returns>Car if found, null otherwise</returns>
    public async Task<Car?> Read(int id)
        => await _cars.FirstOrDefaultAsync(e => e.Id == id);

    /// <summary>
    /// Retrieves all cars from the database
    /// </summary>
    /// <returns>List of all cars</returns>
    public async Task<IList<Car>> ReadAll()
        => await _cars.ToListAsync();

    /// <summary>
    /// Creates a new car in the database
    /// </summary>
    /// <param name="entity">Car entity to create</param>
    /// <returns>Created car entity</returns>
    public async Task<Car> Create(Car entity)
    {
        var result = await _cars.AddAsync(entity);
        await context.SaveChangesAsync();
        return result.Entity;
    }

    /// <summary>
    /// Updates an existing car in the database
    /// </summary>
    /// <param name="entity">Car entity with updated data</param>
    /// <returns>Updated car entity</returns>
    public async Task<Car> Update(Car entity)
    {
        _cars.Update(entity);
        await context.SaveChangesAsync();
        return entity;
    }

    /// <summary>
    /// Deletes a car by its identifier
    /// </summary>
    /// <param name="id">Car identifier</param>
    /// <returns>True if deletion was successful, false otherwise</returns>
    public async Task<bool> Delete(int id)
    {
        var entity = await Read(id);
        if (entity == null) return false;

        _cars.Remove(entity);
        await context.SaveChangesAsync();
        return true;
    }
}
