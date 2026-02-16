namespace BuildingMaterialsCatalog.Middleware
{
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
                await HandleValidationExceptionAsync(context, ex);
            }
            catch (KeyNotFoundException ex)
            {
                await HandleNotFoundExceptionAsync(context, ex);
            }
            catch (Exception ex)
            {
                await HandleGenericExceptionAsync(context, ex);
            }
        }

        private async Task HandleValidationExceptionAsync(HttpContext context, ValidationException ex)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            context.Response.ContentType = "application/json";

            var requestId = context.Items["RequestId"]?.ToString() ?? Guid.NewGuid().ToString();
            var errorResponse = new ErrorResponse("VALIDATION_ERROR", ex.Message, requestId);

            _logger.LogWarning(ex, "Validation error for request {RequestId}", requestId);
            await context.Response.WriteAsJsonAsync(errorResponse);
        }

        private async Task HandleNotFoundExceptionAsync(HttpContext context, KeyNotFoundException ex)
        {
            context.Response.StatusCode = StatusCodes.Status404NotFound;
            context.Response.ContentType = "application/json";

            var requestId = context.Items["RequestId"]?.ToString() ?? Guid.NewGuid().ToString();
            var errorResponse = new ErrorResponse("NOT_FOUND", ex.Message, requestId);

            _logger.LogWarning(ex, "Resource not found for request {RequestId}", requestId);
            await context.Response.WriteAsJsonAsync(errorResponse);
        }

        private async Task HandleGenericExceptionAsync(HttpContext context, Exception ex)
        {
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Response.ContentType = "application/json";

            var requestId = context.Items["RequestId"]?.ToString() ?? Guid.NewGuid().ToString();
            var errorResponse = new ErrorResponse("INTERNAL_ERROR", "An unexpected error occurred", requestId);

            _logger.LogError(ex, "Unhandled exception for request {RequestId}", requestId);
            await context.Response.WriteAsJsonAsync(errorResponse);
        }
    }

    public class ValidationException : Exception
    {
        public ValidationException(string message) : base(message) { }
    }
}