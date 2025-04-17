using Microsoft.EntityFrameworkCore;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<AuthUser> CreateAsync(AuthUser user)
    {
        _context.AuthUser.Add(user);
        await _context.SaveChangesAsync();
        return user;
    }

    public Task<bool> CheckDuplicatedUsername(string username)
    {
        return _context.AuthUser.AnyAsync(u => u.UserName == username);
    }

    public Task<bool> CheckDuplicatedEmail(string email)
    {
        return _context.AuthUser.AnyAsync(u => u.Email == email);
    }
}
