public interface IRefreshTokenService
{
    Task<RefreshToken> CreateAsync(int userId);
    Task<RefreshToken?> GetAsync(string token);
    Task MarkAsUsedAsync(RefreshToken token);
    Task InvalidateUserTokensAsync(int userId);
    Task RemoveExpiredTokensAsync();
}