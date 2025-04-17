/***************************

        AuthConverter

***************************/
// Description
// : 인증 관련 객체 변환기
//
// Author : ChoiHyunSan
public class AuthConverter
{
    public static RegisterResponse toRegisterResponse(AuthUser user)
    {
        return new RegisterResponse
        {
            UserName = user.Username,
            Email = user.Email,
            CreatedAt = user.CreatedAt
        };
    }
}