public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;

    public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context); // 다음 미들웨어 or 컨트롤러 호출
        }
        catch (ApiException ex)
        {
            _logger.LogWarning(ex, "Handled API Exception");
            await HandleExceptionAsync(context, ex.StatusCode, ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled Exception");
            await HandleExceptionAsync(context, 500, "서버 오류가 발생했습니다.");
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, int statusCode, string message)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;

        var response = new
        {
            error = message,
            status = statusCode,
            timestamp = DateTime.UtcNow
        };

        await context.Response.WriteAsJsonAsync(response);
    }
}
