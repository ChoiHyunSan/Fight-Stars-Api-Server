public class AuthErrorCodes
{
    public static readonly ErrorDetail EmailAlreadyExists = new("이미 사용 중인 이메일입니다.", 409);
    public static readonly ErrorDetail UsernameAlreadyExists = new("이미 사용 중인 사용자 이름입니다.", 409);
    public static readonly ErrorDetail GoogleAccountAlreadyLinked = new("이미 연동된 Google 계정입니다.", 409);
    public static readonly ErrorDetail EmailMissing = new("이메일이 누락되었습니다.", 400);
    public static readonly ErrorDetail InvalidCredentials = new("이메일 또는 비밀번호가 잘못되었습니다.", 401);
}

