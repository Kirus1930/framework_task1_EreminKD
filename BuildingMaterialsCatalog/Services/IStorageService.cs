using BuildingMaterialsCatalog.Models;

namespace BuildingMaterialsCatalog.Services;

public interface IStorageService
{
    IEnumerable<BuildingMaterial> GetAll();
    BuildingMaterial? GetById(Guid id);
    Task<BuildingMaterial> CreateAsync(BuildingMaterialCreateDto dto);
}