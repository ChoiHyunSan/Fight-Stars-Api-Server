/***************************

         PasswordUtils

***************************/
// Description
// : 비밀번호 해싱 및 검증을 위한 유틸리티 클래스입니다.  
//   BCrypt.Net 라이브러리를 사용하여 비밀번호를 해싱하고 검증합니다.
//
// Author : ChoiHyunSan
public class PasswordUtils
{
    public static string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    public static bool VerifyPassword(string password, string hashedPassword)
    {
        return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
    }
}
