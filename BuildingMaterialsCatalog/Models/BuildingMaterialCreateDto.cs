namespace BuildingMaterialsCatalog.Models;

public class BuildingMaterialCreateDto
{
    public string Name { get; set; } = string.Empty;
    public string UnitOfMeasure { get; set; } = string.Empty;
    public decimal PricePerUnit { get; set; }
    public int QuantityInStock { get; set; }
}