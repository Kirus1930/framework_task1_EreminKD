namespace BuildingMaterialsCatalog.Middleware
{
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
            var requestId = Guid.NewGuid().ToString();
            context.Items["RequestId"] = requestId;

            var request = context.Request;
            _logger.LogInformation(
                "Request {RequestId}: {Method} {Path} at {Time}",
                requestId,
                request.Method,
                request.Path,
                DateTime.UtcNow);

            // Сохранение оригинального потока ответа
            var originalBodyStream = context.Response.Body;
            using var responseBody = new MemoryStream();
            context.Response.Body = responseBody;

            await _next(context);

            // Логирование ответа
            responseBody.Seek(0, SeekOrigin.Begin);
            var responseText = await new StreamReader(responseBody).ReadToEndAsync();
            responseBody.Seek(0, SeekOrigin.Begin);
            await responseBody.CopyToAsync(originalBodyStream);

            _logger.LogInformation(
                "Response {RequestId}: Status {StatusCode} at {Time}",
                requestId,
                context.Response.StatusCode,
                DateTime.UtcNow);
        }
    }
}