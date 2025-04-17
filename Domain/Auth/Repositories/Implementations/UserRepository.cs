using Microsoft.EntityFrameworkCore;

/***************************

       UserRepository

***************************/
// Description
// : IUserRepository 인터페이스를 구현한 클래스입니다.
//   AuthUser 엔티티에 대한 CRUD 작업을 수행합니다.
//
// Author : ChoiHyunSan
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
