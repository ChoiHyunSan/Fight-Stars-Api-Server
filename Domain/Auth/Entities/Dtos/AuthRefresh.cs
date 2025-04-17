
/***************************

       RefreshRequest

***************************/
// Description : 로그인 요청 DTO
// Author : ChoiHyunSan
public class RefreshRequest
{
    public string RefreshToken { get; set; } = string.Empty;
}

/***************************

      RefreshResponse

***************************/
// Description : 로그인 요청 DTO
// Author : ChoiHyunSan
public class RefreshResponse
{
    public string AccessToken { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
}