using Microsoft.EntityFrameworkCore;

public class RefreshTokenService : IRefreshTokenService
{
    private readonly AppDbContext _context;

    public RefreshTokenService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<RefreshToken> CreateAsync(string userId)
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

    public async Task InvalidateUserTokensAsync(string userId)
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
}
