namespace BuildingMaterialsCatalog.Models;

public class ErrorResponse
{
    public string ErrorCode { get; set; }
    public string Message { get; set; }
    public string RequestId { get; set; }
    public DateTime Timestamp { get; set; }

    public ErrorResponse(string errorCode, string message, string requestId)
    {
        ErrorCode = errorCode;
        Message = message;
        RequestId = requestId;
        Timestamp = DateTime.UtcNow;
    }
}