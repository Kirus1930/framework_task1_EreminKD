using CatalogService.Domain.Enums;

namespace CatalogService.Application.DTOs;

public class CreateItemRequest
{
    public string Name { get; set; } = string.Empty;
    public UnitOfMeasure Unit { get; set; }
    public decimal PricePerUnit { get; set; }
    public decimal QuantityInStock { get; set; }
}
