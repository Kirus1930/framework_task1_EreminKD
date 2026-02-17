using BuildingMaterialsCatalog.Domain.Entities;

namespace BuildingMaterialsCatalog.Infrastructure.Repositories;

public interface IItemRepository
{
    IEnumerable<Item> GetAll();
    Item? GetById(Guid id);
    Item Add(Item item);
}
