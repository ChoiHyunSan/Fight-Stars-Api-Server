public class ErrorDetail
{
    public string Message { get; }
    public int StatusCode { get; }

    public ErrorDetail(string message, int statusCode)
    {
        Message = message;
        StatusCode = statusCode;
    }
}
