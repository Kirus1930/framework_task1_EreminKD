using BuildingMaterialsCatalog.Models;

namespace BuildingMaterialsCatalog.Validators;

public class BuildingMaterialValidator
{
    private static readonly string[] AllowedUnits = { "kg", "l", "m", "pcs" };

    public void ValidateCreateDto(BuildingMaterialCreateDto dto)
    {
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(dto.Name))
            errors.Add("Name is required.");
        else if (dto.Name.Length < 2)
            errors.Add("Name must be at least 2 characters long.");
        else if (dto.Name.Length > 100)
            errors.Add("Name must not exceed 100 characters.");

        if (!AllowedUnits.Contains(dto.UnitOfMeasure.ToLower()))
            errors.Add($"Unit of measure must be one of: {string.Join(", ", AllowedUnits)}.");

        if (dto.PricePerUnit <= 0)
            errors.Add("Price per unit must be a positive number.");

        if (dto.QuantityInStock < 0)
            errors.Add("Quantity in stock cannot be negative.");

        if (errors.Any())
            throw new ValidationException(string.Join(" ", errors));
    }
}

// Кастомное исключение для валидации
public class ValidationException : Exception
{
    public ValidationException(string message) : base(message) { }
}