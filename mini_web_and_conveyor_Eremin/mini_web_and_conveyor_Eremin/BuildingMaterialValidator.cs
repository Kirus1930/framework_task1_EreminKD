namespace BuildingMaterialsCatalog.Validators
{
    public class BuildingMaterialValidator
    {
        public void ValidateCreateDto(BuildingMaterialCreateDto dto)
        {
            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(dto.Name))
                errors.Add("Name is required");
            else if (dto.Name.Length > 100)
                errors.Add("Name must be less than 100 characters");

            var allowedUnits = new[] { "kg", "l", "m", "pcs" };
            if (!allowedUnits.Contains(dto.UnitOfMeasure.ToLower()))
                errors.Add($"Unit of measure must be one of: {string.Join(", ", allowedUnits)}");

            if (dto.PricePerUnit <= 0)
                errors.Add("Price per unit must be positive");

            if (dto.QuantityInStock < 0)
                errors.Add("Quantity in stock cannot be negative");

            if (dto.Name.Length < 2)
                errors.Add("Name must be at least 2 characters long");

            if (errors.Any())
                throw new ValidationException(string.Join("; ", errors));
        }
    }
}