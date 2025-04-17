/***************************

    IRefreshTokenService

***************************/
// Description
// : RefreshToken 엔티티에 대한 CRUD 작업을 수행하는 인터페이스입니다.
//   RefreshToken은 AccessToken과 함께 발급되며,
//   AccessToken이 만료된 경우 RefreshToken을 사용하여 새로운 AccessToken을 발급받을 수 있습니다.
//
// Author : ChoiHyunSan
public interface IRefreshTokenService
{
    Task<RefreshToken> CreateAsync(int userId);
    Task<RefreshToken?> GetAsync(string token);
    Task MarkAsUsedAsync(RefreshToken token);
    Task InvalidateUserTokensAsync(int userId);
    Task RemoveExpiredTokensAsync();
}