using CarRentalService.Domain;
using CarRentalService.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace CarRentalService.Infrastructure.EfCore.Repositories;

/// <summary>
/// Repository implementation for CarModel entities using Entity Framework Core with MongoDB
/// </summary>
public class CarModelEfCoreRepository(CarRentalDbContext context) : IRepository<CarModel, int>
{
    private readonly DbSet<CarModel> _carModels = context.CarModels!;

    /// <summary>
    /// Retrieves a car model by its identifier
    /// </summary>
    /// <param name="id">Car model identifier</param>
    /// <returns>Car model if found, null otherwise</returns>
    public async Task<CarModel?> Read(int id)
        => await _carModels.FindAsync(id);

    /// <summary>
    /// Retrieves all car models from the database
    /// </summary>
    /// <returns>List of all car models</returns>
    public async Task<IList<CarModel>> ReadAll()
        => await _carModels.ToListAsync();

    /// <summary>
    /// Creates a new car model in the database
    /// </summary>
    /// <param name="entity">Car model entity to create</param>
    /// <returns>Created car model entity</returns>
    public async Task<CarModel> Create(CarModel entity)
    {
        var result = await _carModels.AddAsync(entity);
        await context.SaveChangesAsync();
        return result.Entity;
    }

    /// <summary>
    /// Updates an existing car model in the database
    /// </summary>
    /// <param name="entity">Car model entity with updated data</param>
    /// <returns>Updated car model entity</returns>
    public async Task<CarModel> Update(CarModel entity)
    {
        _carModels.Update(entity);
        await context.SaveChangesAsync();
        return entity;
    }

    /// <summary>
    /// Deletes a car model by its identifier
    /// </summary>
    /// <param name="id">Car model identifier</param>
    /// <returns>True if deletion was successful, false otherwise</returns>
    public async Task<bool> Delete(int id)
    {
        var entity = await Read(id);
        if (entity == null) return false;

        _carModels.Remove(entity);
        await context.SaveChangesAsync();
        return true;
    }
}
