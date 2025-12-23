using CarRentalService.API.DTOs;

namespace CarRentalService.API.Interfaces;

/// <summary>
/// Generic service interface for CRUD operations
/// Based on best practices from PR #234
/// </summary>
/// <typeparam name="TDto">DTO for reading/returning data</typeparam>
/// <typeparam name="TCreateUpdateDto">DTO for creating/updating data</typeparam>
public interface IApplicationService<TDto, in TCreateUpdateDto>
{
    /// <summary>
    /// Returns all entities
    /// </summary>
    List<TDto> ReadAll();

    /// <summary>
    /// Returns entity by ID
    /// </summary>
    TDto? Read(int id);

    /// <summary>
    /// Creates new entity
    /// </summary>
    TDto? Create(TCreateUpdateDto dto);

    /// <summary>
    /// Updates existing entity
    /// </summary>
    bool Update(TCreateUpdateDto dto, int id);

    /// <summary>
    /// Deletes entity by ID
    /// </summary>
    bool Delete(int id);
}
