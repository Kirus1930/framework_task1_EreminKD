namespace BuildingMaterialsCatalog.Models;

public class BuildingMaterial
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string UnitOfMeasure { get; set; } = string.Empty; // kg, l, m, pcs
    public decimal PricePerUnit { get; set; }
    public int QuantityInStock { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}