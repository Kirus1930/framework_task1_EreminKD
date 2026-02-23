using System.Diagnostics;
using BuildingMaterialsCatalog.Models;

namespace BuildingMaterialsCatalog.Middleware;

public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestLoggingMiddleware> _logger;

    public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Генерируем уникальный ID запроса и сохраняем в Items
        var requestId = Guid.NewGuid().ToString();
        context.Items["RequestId"] = requestId;

        // Логируем входящий запрос
        _logger.LogInformation("Request {RequestId}: {Method} {Path} at {Time}",
            requestId, context.Request.Method, context.Request.Path, DateTime.UtcNow);

        // Перехватываем ответ для логирования
        var originalBodyStream = context.Response.Body;
        using var responseBody = new MemoryStream();
        context.Response.Body = responseBody;

        await _next(context);

        // Логируем ответ
        responseBody.Seek(0, SeekOrigin.Begin);
        var responseText = await new StreamReader(responseBody).ReadToEndAsync();
        responseBody.Seek(0, SeekOrigin.Begin);
        await responseBody.CopyToAsync(originalBodyStream);

        _logger.LogInformation("Response {RequestId}: Status {StatusCode} at {Time}",
            requestId, context.Response.StatusCode, DateTime.UtcNow);
    }
}