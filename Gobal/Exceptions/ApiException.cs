/***************************

        ApiException

***************************/
// Description
// : Api Exception 클래스는 API 호출 중 발생하는 예외를 나타냅니다.
//   상황에 맞는 ErrorDetail을 정의하여 예외를 던지면, GlobalExceptionMiddleware에서 처리합니다.
//
// Author : ChoiHyunSan
public class ApiException : Exception
{
    public int StatusCode { get; }
    public ApiException(ErrorDetail error) : base(error.Message)
    {
        StatusCode = error.StatusCode;
    }
}
