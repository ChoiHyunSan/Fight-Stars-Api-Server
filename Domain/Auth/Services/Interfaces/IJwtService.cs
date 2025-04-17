/***************************

        IJwtService

***************************/
// Description
// : Jwt 토큰 발급 및 검증을 위한 서비스 인터페이스입니다.
//
// Author : ChoiHyunSan
public interface IJwtService
{
    string GenerateToken(int userId, string role);
}