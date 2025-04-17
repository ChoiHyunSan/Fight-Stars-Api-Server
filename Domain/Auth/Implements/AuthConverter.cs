
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