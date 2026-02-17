using CatalogService.Domain.Entities;

namespace CatalogService.Infrastructure.Repositories;

public interface IItemRepository
{
    IEnumerable<Item> GetAll();
    Item? GetById(Guid id);
    Item Add(Item item);
}
