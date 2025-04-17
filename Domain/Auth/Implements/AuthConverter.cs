public class AuthConverter
{
    public static RegisterResponse toRegisterResponse(AuthUser user)
    {
        return new RegisterResponse
        {
            UserName = user.UserName,
            Email = user.Email,
            CreatedAt = user.CreatedAt
        };
    }
}