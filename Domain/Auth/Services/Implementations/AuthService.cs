
public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;

    public AuthService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<AuthUser> RegisterWithLocalAsync(string username, string email, string password)
    {
        if (await _userRepository.CheckDuplicatedUsername(username))
        {
            throw new ApiException(AuthErrorCodes.UsernameAlreadyExists);
        }

        if(await _userRepository.CheckDuplicatedEmail(email))
        {
            throw new ApiException(AuthErrorCodes.EmailAlreadyExists);
        }

        var user = AuthUser.CreateWithLocal(username, email, PasswordUtils.HashPassword(password));
        return await _userRepository.CreateAsync(user);
    }
}
