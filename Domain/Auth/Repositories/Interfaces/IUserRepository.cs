
public interface IUserRepository
{ 
    Task<AuthUser> CreateAsync(AuthUser user);
    Task<bool> CheckDuplicatedUsername(string username);
    Task<bool> CheckDuplicatedEmail(string email);
    Task<AuthUser> FindByUsername(string userName);
    Task<AuthUser> FindByUserId(int userId);
}
