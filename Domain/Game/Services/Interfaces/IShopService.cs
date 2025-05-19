public interface IShopService
{
    Task<BuyCharacterResponse> BuyCharacter(int userId, BuyCharacterRequest request);
    Task<BuySkinResponse> BuySkin(int userId, BuySkinRequest request);
}
