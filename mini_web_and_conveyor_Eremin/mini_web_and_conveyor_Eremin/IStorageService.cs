namespace BuildingMaterialsCatalog.Services
{
    public interface IStorageService
    {
        IEnumerable<BuildingMaterial> GetAll();
        BuildingMaterial? GetById(Guid id);
        BuildingMaterial Create(BuildingMaterialCreateDto dto);
        bool Delete(Guid id);
    }
}