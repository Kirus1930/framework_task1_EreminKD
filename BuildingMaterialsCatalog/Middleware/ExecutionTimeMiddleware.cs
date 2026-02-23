using System.Diagnostics;

namespace BuildingMaterialsCatalog.Middleware;

public class ExecutionTimeMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExecutionTimeMiddleware> _logger;

    public ExecutionTimeMiddleware(RequestDelegate next, ILogger<ExecutionTimeMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var stopwatch = Stopwatch.StartNew();
        await _next(context);
        stopwatch.Stop();

        var requestId = context.Items["RequestId"]?.ToString() ?? "unknown";
        _logger.LogInformation("Request {RequestId} executed in {ElapsedMilliseconds} ms",
            requestId, stopwatch.ElapsedMilliseconds);
    }
}