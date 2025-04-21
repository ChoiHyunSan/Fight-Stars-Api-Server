/***************************

         AuthService

***************************/
// Description
// : IAuthService 인터페이스를 구현한 AuthService 클래스입니다.
//   AuthService는 사용자 인증 및 권한 부여를 처리합니다.    
//
// Author : ChoiHyunSan
using System.Transactions;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtService _jwtService;
    private readonly IRefreshTokenService _refreshTokenService;
    private readonly IGameUserInitializer _gameUserInitializer;
    public AuthService(
        IUserRepository userRepository, 
        IJwtService jwtService, 
        IRefreshTokenService refreshTokenService,
        IGameUserInitializer gameUserInitializer)
    {
        _userRepository = userRepository;
        _jwtService = jwtService;
        _refreshTokenService = refreshTokenService;
        _gameUserInitializer = gameUserInitializer;
    }

    public async Task<LoginResponse> Login(string username, string password)
    {
        // 사용자 이름과 비밀번호로 사용자 인증
        AuthUser user = await _userRepository.FindByUsername(username);
        if(user == null)
        {
            throw new ApiException(AuthErrorCodes.UserNotFound);
        }
        user.Verify(password);

        // 사용자 인증 성공 시 JWT 토큰 생성 및 저장
        string accessToken = _jwtService.GenerateToken(user.Id, user.Role);
        var refreshToken = await _refreshTokenService.CreateAsync(user.Id);

        // 사용자 인증 응답 생성 및 반환
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
        // 리프레시 토큰 조회 및 검증
        var refreshToken = await _refreshTokenService.GetAsync(token);
        if(refreshToken == null)
        {
            throw new ApiException(AuthErrorCodes.RefreshTokenNotFound);
        }
        refreshToken.Verify();
        await _refreshTokenService.MarkAsUsedAsync(refreshToken);

        // 리프레시 토큰 정보에 담긴 유저 정보로 새로운 토큰 생성
        var user = await _userRepository.FindByUserId(refreshToken.UserId);
        if(user == null)
        {
            throw new ApiException(AuthErrorCodes.UserNotFound);
        }
        var newAccessToken = _jwtService.GenerateToken(refreshToken.UserId, user.Role);
        var newRefreshToken = await _refreshTokenService.CreateAsync(user.Id);

        // 새로운 토큰 정보로 응답 생성
        return new RefreshResponse
        {
            AccessToken = newAccessToken,
            RefreshToken = newRefreshToken.Token
        };
    }

    public async Task<AuthUser> RegisterAsync(string username, string email, string password)
    {
        // 중복되는 사용자 이름 확인
        if (await _userRepository.CheckDuplicatedUsername(username))
        {
            throw new ApiException(AuthErrorCodes.UsernameAlreadyExists);
        }

        // 중복되는 이메일 확인
        if (await _userRepository.CheckDuplicatedEmail(email))
        {
            throw new ApiException(AuthErrorCodes.EmailAlreadyExists);
        }

        // 사용자 생성 및 응답 반환
        var user = AuthUser.CreateWithLocal(username, email, PasswordUtils.HashPassword(password));
        var authUser = await _userRepository.CreateAsync(user);
        await _gameUserInitializer.InitializeNewUserAsync(authUser.Id, username);

        return authUser;
    }
}
