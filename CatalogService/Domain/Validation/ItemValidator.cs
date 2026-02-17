using BuildingMaterialsCatalog.Application.DTOs;
using BuildingMaterialsCatalog.Domain.Exceptions;

namespace BuildingMaterialsCatalog.Domain.Validation;

public static class ItemValidator
{
    public static void Validate(CreateItemRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
            throw new DomainException("INVALID_NAME", "Название не может быть пустым");

        if (request.PricePerUnit <= 0)
            throw new DomainException("INVALID_PRICE", "Цена не может быть отрицательной или нулевой");

        if (request.QuantityInStock < 0)
            throw new DomainException("INVALID_QUANTITY", "Количество не может быть отрицательным");
    }
}
