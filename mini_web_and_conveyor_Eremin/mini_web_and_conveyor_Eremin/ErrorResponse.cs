namespace BuildingMaterialsCatalog.Models
{
    public class ErrorResponse
    {
        public string ErrorCode { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public string RequestId { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }

        public ErrorResponse(string errorCode, string message, string requestId)
        {
            ErrorCode = errorCode;
            Message = message;
            RequestId = requestId;
            Timestamp = DateTime.UtcNow;
        }
    }
}