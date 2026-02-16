var builder = WebApplication.CreateBuilder(args);

// Добавление сервисов
builder.Services.AddControllers();
builder.Services.AddSingleton<InMemoryStorageService>();
builder.Services.AddSingleton<BuildingMaterialValidator>();
builder.Services.AddLogging(configure =>
    configure.AddConsole().AddDebug());

var app = builder.Build();

// Конфигурация конвейера
app.UseMiddleware<RequestLoggingMiddleware>();
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseMiddleware<ExecutionTimeMiddleware>();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

// Начальные данные
using (var scope = app.Services.CreateScope())
{
    var storage = scope.ServiceProvider.GetRequiredService<InMemoryStorageService>();
    await storage.CreateAsync(new BuildingMaterialCreateDto
    {
        Name = "Цемент М500",
        UnitOfMeasure = "kg",
        PricePerUnit = 50,
        QuantityInStock = 1000
    });
    await storage.CreateAsync(new BuildingMaterialCreateDto
    {
        Name = "Песок речной",
        UnitOfMeasure = "kg",
        PricePerUnit = 15,
        QuantityInStock = 5000
    });
}

app.Run();