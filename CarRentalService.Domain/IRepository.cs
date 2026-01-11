namespace CarRentalService.Domain;

/// <summary>
/// Generic repository interface for basic CRUD operations
/// </summary>
/// <typeparam name="T">Entity type</typeparam>
/// <typeparam name="TId">Entity identifier type</typeparam>
public interface IRepository<T, TId> where T : class
{
    /// <summary>
    /// Creates a new entity in the repository
    /// </summary>
    /// <param name="entity">Entity to create</param>
    /// <returns>Created entity</returns>
    public Task<T> Create(T entity);

    /// <summary>
    /// Deletes an entity by its identifier
    /// </summary>
    /// <param name="id">Entity identifier</param>
    /// <returns>True if deletion was successful, false otherwise</returns>
    public Task<bool> Delete(TId id);

    /// <summary>
    /// Retrieves an entity by its identifier
    /// </summary>
    /// <param name="id">Entity identifier</param>
    /// <returns>Entity if found, null otherwise</returns>
    public Task<T?> Read(TId id);

    /// <summary>
    /// Retrieves all entities from the repository
    /// </summary>
    /// <returns>List of all entities</returns>
    public Task<IList<T>> ReadAll();
    
    /// <summary>
    /// Updates an existing entity
    /// </summary>
    /// <param name="entity">Entity with updated data</param>
    /// <returns>Updated entity</returns>
    public Task<T> Update(T entity);
}
