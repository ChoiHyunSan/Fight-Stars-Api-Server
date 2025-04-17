public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtService _jwtService;
    private readonly IRefreshTokenService _refreshTokenService;

    public AuthService(
        IUserRepository userRepository, 
        IJwtService jwtService, 
        IRefreshTokenService refreshTokenService)
    {
        _userRepository = userRepository;
        _jwtService = jwtService;
        _refreshTokenService = refreshTokenService;
    }

    public async Task<LoginResponse> Login(string username, string password)
    {
        AuthUser user = await _userRepository.FindByUsername(username);
        if(user == null || !PasswordUtils.VerifyPassword(password, user.Password))
        {
            throw new ApiException(AuthErrorCodes.InvalidCredentials);
        }

        string accessToken = _jwtService.GenerateToken(user.Id, user.Role);
        var refreshToken = await _refreshTokenService.CreateAsync(user.Id);

        var loginResponse = new LoginResponse
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken.Token,
            UserId = user.Id,
            UserName = user.Username,
            Role = user.Role
        };

        return loginResponse;
    }

    public async Task<RefreshResponse> Refresh(string token)
    {
        var refreshToken = await _refreshTokenService.GetAsync(token);
        if(refreshToken == null || refreshToken.IsUsed || refreshToken.IsRevoked || refreshToken.ExpiresAt < DateTime.UtcNow)
        {
            throw new ApiException(AuthErrorCodes.InvalidToken);
        }
        await _refreshTokenService.MarkAsUsedAsync(refreshToken);

        var user = await _userRepository.FindByUserId(refreshToken.UserId);
        if(user == null)
        {
            throw new ApiException(AuthErrorCodes.UserNotFound);
        }

        var newAccessToken = _jwtService.GenerateToken(refreshToken.UserId, user.Role);
        var newRefreshToken = _refreshTokenService.CreateAsync(user.Id);

        return new RefreshResponse
        {
            accessToken = newAccessToken
        };
    }

    public async Task<AuthUser> RegisterAsync(string username, string email, string password)
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
