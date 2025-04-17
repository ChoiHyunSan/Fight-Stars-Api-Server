/***************************

         ErrorDetail

***************************/
// Description
// : 에러에서 발생한 상세 정보를 담고 있는 클래스입니다.
//   상태 코드와 메시지를 포함합니다.
//
// Author : ChoiHyunSan
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
