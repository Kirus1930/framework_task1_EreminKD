using BuildingMaterialsCatalog.Domain.Enums;

namespace BuildingMaterialsCatalog.Domain.Entities;

public class Item
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public string Name { get; init; } = string.Empty;
    public UnitOfMeasure Unit { get; init; }
    public decimal PricePerUnit { get; init; }
    public decimal QuantityInStock { get; init; }
}
