/***************************

  GlobalExceptionMiddleware

***************************/
// Description
// : API 호출 중 발생하는 예외를 처리하는 미들웨어입니다.
//   ApiException을 처리하여 클라이언트에게 적절한 응답을 반환합니다.
//   그 외의 예외는 500 상태 코드와 함께 서버 오류 메시지를 반환합니다.
//
// Author : ChoiHyunSan
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
