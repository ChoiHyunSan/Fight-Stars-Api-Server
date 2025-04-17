public interface IAuthService
{
    Task<LoginResponse> Login(string userName, string password);
    Task<RefreshResponse> Refresh(string refreshToken);
    Task<AuthUser> RegisterAsync(string username, string email, string password);
}
