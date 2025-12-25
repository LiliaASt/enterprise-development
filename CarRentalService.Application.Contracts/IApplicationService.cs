namespace CarRentalService.Application.Contracts;

/// <summary>
/// Generic service interface for CRUD operations
/// </summary>
/// <typeparam name="TDto">DTO for reading/returning data</typeparam>
/// <typeparam name="TCreateUpdateDto">DTO for creating/updating data</typeparam>
public interface IApplicationService<TDto, in TCreateUpdateDto>
{
    /// <summary>
    /// Returns all entities
    /// </summary>
    public List<TDto> ReadAll();

    /// <summary>
    /// Returns entity by ID
    /// </summary>
    public TDto? Read(int id);

    /// <summary>
    /// Creates new entity
    /// </summary>
    public TDto? Create(TCreateUpdateDto dto);

    /// <summary>
    /// Updates existing entity
    /// </summary>
    public bool Update(TCreateUpdateDto dto, int id);

    /// <summary>
    /// Deletes entity by ID
    /// </summary>
    public bool Delete(int id);
}
