public interface IRefreshTokenService
{
    Task<RefreshToken> CreateAsync(string userId);
    Task<RefreshToken?> GetAsync(string token);
    Task MarkAsUsedAsync(RefreshToken token);
    Task InvalidateUserTokensAsync(string userId);
}