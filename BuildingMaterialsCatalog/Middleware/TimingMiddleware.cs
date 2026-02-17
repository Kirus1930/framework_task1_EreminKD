using System.Diagnostics;

namespace BuildingMaterialsCatalog.Middleware;

public class TimingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<TimingMiddleware> _logger;

    public TimingMiddleware(
        RequestDelegate next,
        ILogger<TimingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        var stopwatch = Stopwatch.StartNew();

        await _next(context);

        stopwatch.Stop();

        var requestId = context.Items["RequestId"]?.ToString();

        _logger.LogInformation(
            "Request {RequestId} executed in {Elapsed} ms",
            requestId,
            stopwatch.ElapsedMilliseconds);
    }
}
