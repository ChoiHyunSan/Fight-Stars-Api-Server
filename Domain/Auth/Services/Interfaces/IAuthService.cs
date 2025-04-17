public interface IAuthService
{
    Task<LoginResponse> Login(string userName, string password);
    Task<AuthUser> RegisterAsync(string username, string email, string password);
}
