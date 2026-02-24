using System.Collections.Concurrent;
using BuildingMaterialsCatalog.Models;

namespace BuildingMaterialsCatalog.Services;

public class InMemoryStorageService : IStorageService
{
    private readonly ConcurrentDictionary<Guid, BuildingMaterial> _storage = new();
    private readonly SemaphoreSlim _semaphore = new(1, 1); // защита от одновременного создания

    public IEnumerable<BuildingMaterial> GetAll() => _storage.Values;

    public BuildingMaterial? GetById(Guid id) => _storage.TryGetValue(id, out var item) ? item : null;

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

            _storage[material.Id] = material;
            return material;
        }
        finally
        {
            _semaphore.Release();
        }
    }

    public bool Delete(Guid id)
    {
        return _storage.TryRemove(id, out _);
    }
}