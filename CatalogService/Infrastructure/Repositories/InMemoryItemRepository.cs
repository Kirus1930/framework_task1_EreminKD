using System.Collections.Concurrent;
using CatalogService.Domain.Entities;

namespace CatalogService.Infrastructure.Repositories;

public class InMemoryItemRepository : IItemRepository
{
    private readonly ConcurrentDictionary<Guid, Item> _items = new(); // Защита от конфликтов при параллельных POST-запросах

    public IEnumerable<Item> GetAll()
    {
        return _items.Values;
    }

    public Item? GetById(Guid id)
    {
        return _items.TryGetValue(id, out var item) ? item : null;
    }

    public Item Add(Item item)
    {
        if (!_items.TryAdd(item.Id, item))
            throw new Exception("Не удалось добавить элемент");

        return item;
    }
}
