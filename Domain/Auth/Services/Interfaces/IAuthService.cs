/***************************

        IAuthService

***************************/
// Description
// : 인증 서비스 인터페이스입니다.
//   로그인, 리프레시, 회원가입 기능을 제공합니다.
//
// Author : ChoiHyunSan
public interface IAuthService
{
    Task<LoginResponse> Login(string userName, string password);
    Task<RefreshResponse> Refresh(string refreshToken);
    Task<AuthUser> RegisterAsync(string username, string email, string password);
}
