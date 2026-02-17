using BuildingMaterialsCatalog.Application.DTOs;
using BuildingMaterialsCatalog.Domain.Entities;
using BuildingMaterialsCatalog.Domain.Exceptions;
using BuildingMaterialsCatalog.Domain.Validation;
using BuildingMaterialsCatalog.Infrastructure.Repositories;
using BuildingMaterialsCatalog.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IItemRepository, InMemoryItemRepository>();

var app = builder.Build();

// Важен данный порядок!
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseMiddleware<LoggingMiddleware>();
app.UseMiddleware<TimingMiddleware>();

// GET /api/items
app.MapGet("/api/items", (IItemRepository repo) =>
{
    return Results.Ok(repo.GetAll());
});

// GET /api/items/{id}
app.MapGet("/api/items/{id:guid}", (Guid id, IItemRepository repo) =>
{
    var item = repo.GetById(id);

    if (item == null)
        throw new DomainException("NOT_FOUND", "Элемент не найден");

    return Results.Ok(item);
});

// POST /api/items
app.MapPost("/api/items", (CreateItemRequest request, IItemRepository repo) =>
{
    ItemValidator.Validate(request);

    var item = new Item
    {
        Name = request.Name,
        Unit = request.Unit,
        PricePerUnit = request.PricePerUnit,
        QuantityInStock = request.QuantityInStock
    };

    repo.Add(item);

    return Results.Created($"/api/items/{item.Id}", item);
});

app.Run();
