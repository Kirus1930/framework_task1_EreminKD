using CatalogService.Application.DTOs;
using CatalogService.Domain.Exceptions;

namespace CatalogService.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(
        RequestDelegate next,
        ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (DomainException ex)
        {
            await WriteError(context, ex.Code, ex.Message, 400);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception");
            await WriteError(context, "INTERNAL_ERROR",
                "Внутренняя ошибка сервера", 500);
        }
    }

    private async Task WriteError(
        HttpContext context,
        string code,
        string message,
        int statusCode)
    {
        var requestId = context.Items["RequestId"]?.ToString() ?? "";

        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/json";

        var error = new ErrorResponse
        {
            Code = code,
            Message = message,
            RequestId = requestId
        };

        await context.Response.WriteAsJsonAsync(error);
    }
}
