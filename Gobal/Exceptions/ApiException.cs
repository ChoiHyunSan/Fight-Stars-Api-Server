public class ApiException : Exception
{
    public int StatusCode { get; }
    public ApiException(ErrorDetail error) : base(error.Message)
    {
        StatusCode = error.StatusCode;
    }
}
