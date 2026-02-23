using System.Text.Json;
using BuildingMaterialsCatalog.Models;
using BuildingMaterialsCatalog.Validators;

namespace BuildingMaterialsCatalog.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (ValidationException ex)
        {
            await HandleExceptionAsync(context, ex, "VALIDATION_ERROR", ex.Message, StatusCodes.Status400BadRequest);
        }
        catch (KeyNotFoundException ex)
        {
            await HandleExceptionAsync(context, ex, "NOT_FOUND", ex.Message, StatusCodes.Status404NotFound);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception");
            await HandleExceptionAsync(context, ex, "INTERNAL_ERROR", "An internal server error occurred.", StatusCodes.Status500InternalServerError);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception ex, string code, string message, int statusCode)
    {
        var requestId = context.Items["RequestId"]?.ToString() ?? Guid.NewGuid().ToString();
        var errorResponse = new ErrorResponse(code, message, requestId);

        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsJsonAsync(errorResponse);
    }
}