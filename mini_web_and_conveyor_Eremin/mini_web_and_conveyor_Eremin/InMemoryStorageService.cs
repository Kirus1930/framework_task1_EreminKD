using System.Collections.Concurrent;

namespace BuildingMaterialsCatalog.Services
{
    public class InMemoryStorageService : IStorageService
    {
        private readonly ConcurrentDictionary<Guid, BuildingMaterial> _materials = new();
        private readonly SemaphoreSlim _semaphore = new(1, 1); // Защита от конфликтов

        public IEnumerable<BuildingMaterial> GetAll() => _materials.Values;

        public BuildingMaterial? GetById(Guid id)
        {
            _materials.TryGetValue(id, out var material);
            return material;
        }

        public async Task<BuildingMaterial> CreateAsync(BuildingMaterialCreateDto dto)
        {
            await _semaphore.WaitAsync();
            try
            {
                var material = new BuildingMaterial
                {
                    Id = Guid.NewGuid(),
                    Name = dto.Name,
                    UnitOfMeasure = dto.UnitOfMeasure,
                    PricePerUnit = dto.PricePerUnit,
                    QuantityInStock = dto.QuantityInStock,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                _materials[material.Id] = material;
                return material;
            }
            finally
            {
                _semaphore.Release();
            }
        }
    }
}