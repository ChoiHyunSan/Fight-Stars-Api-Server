using Microsoft.EntityFrameworkCore;

/***************************

         JwtService

***************************/
// Description
// : IRefreshTokenService 인터페이스를 구현한 클래스입니다.
//   RefreshToken 엔티티에 대한 CRUD 작업을 수행합니다.
//
// Author : ChoiHyunSan
public class RefreshTokenService : IRefreshTokenService
{
    private readonly AppDbContext _context;

    public RefreshTokenService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<RefreshToken> CreateAsync(int userId)
    {
        var refreshToken = new RefreshToken
        {
            Token = Guid.NewGuid().ToString("N"),
            UserId = userId,
            ExpiresAt = DateTime.UtcNow.AddDays(7)
        };

        _context.Set<RefreshToken>().Add(refreshToken);
        await _context.SaveChangesAsync();
        return refreshToken;
    }

    public async Task<RefreshToken?> GetAsync(string token)
    {
        return await _context.Set<RefreshToken>()
            .FirstOrDefaultAsync(r => r.Token == token);
    }

    public async Task MarkAsUsedAsync(RefreshToken token)
    {
        token.IsUsed = true;
        await _context.SaveChangesAsync();
    }

    public async Task InvalidateUserTokensAsync(int userId)
    {
        var tokens = await _context.Set<RefreshToken>()
            .Where(r => r.UserId == userId && !r.IsUsed && !r.IsRevoked)
            .ToListAsync();

        foreach (var token in tokens)
        {
            token.IsRevoked = true;
        }

        await _context.SaveChangesAsync();
    }

    public async Task RemoveExpiredTokensAsync()
    {
        var expired = await _context.Set<RefreshToken>()
            .Where(r => r.ExpiresAt <= DateTime.UtcNow)
            .ToListAsync();

        if (expired.Any())
        {
            _context.RemoveRange(expired);
            await _context.SaveChangesAsync();
        }
    }
}
