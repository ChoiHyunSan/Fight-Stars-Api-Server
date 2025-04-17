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
        return _context.AuthUser.AnyAsync(u => u.Username == username);
    }

    public Task<bool> CheckDuplicatedEmail(string email)
    {
        return _context.AuthUser.AnyAsync(u => u.Email == email);
    }

    public Task<AuthUser> FindByUsername(string username)
    {
        return _context.AuthUser.FirstAsync(u => u.Username == username);
    }

    public Task<AuthUser> FindByUserId(int userId)
    {
        return _context.AuthUser.FirstAsync(u => u.Id == userId);
    }
}
