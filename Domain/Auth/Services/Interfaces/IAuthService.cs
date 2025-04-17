public interface IAuthService
{
    Task<AuthUser> RegisterWithLocalAsync(string username, string email, string password);
}
