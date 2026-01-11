namespace CarRentalService.Application.Contracts;

/// <summary>
/// Generic service interface defining basic CRUD operations for entities
/// </summary>
/// <typeparam name="TDto">DTO type for entity representation</typeparam>
/// <typeparam name="TCreateUpdateDto">DTO type for create/update operations</typeparam>
/// <typeparam name="TId">Type of the entity's primary key</typeparam>
public interface IApplicationService<TDto, TCreateUpdateDto, TId>
    where TDto : class
    where TCreateUpdateDto : class
{
    /// <summary>
    /// Creates a new entity
    /// </summary>
    public Task<TDto> Create(TCreateUpdateDto dto);

    /// <summary>
    /// Retrieves a specific entity by its identifier
    /// </summary>
    public Task<TDto?> Get(TId id);

    /// <summary>
    /// Retrieves all entities
    /// </summary>
    public Task<IList<TDto>> GetAll();

    /// <summary>
    /// Updates an existing entity
    /// </summary>
    public Task<TDto> Update(TCreateUpdateDto dto, TId id);

    /// <summary>
    /// Deletes an entity by its identifier
    /// </summary>
    public Task<bool> Delete(TId id);
}
