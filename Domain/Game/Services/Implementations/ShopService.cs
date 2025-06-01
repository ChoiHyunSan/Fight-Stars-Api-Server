
using Microsoft.EntityFrameworkCore;

public class ShopService : IShopService
{
    private readonly AppDbContext _context;

    public ShopService(
        AppDbContext context)
    {
        _context = context;
    }

    public async Task<BuyCharacterResponse> BuyCharacter(int userId, BuyCharacterRequest request)
    {
        Console.WriteLine($"BuyCharacterRequest ID: {request.CharacterId}, CurrentGold : {request.CurrentGold}");

        var gameUser = await _context.GameUsers
            .Include(u => u.Currency)
            .Include(u => u.Brawlers)
            .FirstOrDefaultAsync(u => u.AccountId == userId);

        if (gameUser == null)
        {
            Console.WriteLine($"GameUser Not Found, Account ID : {userId}");
            throw new ApiException(GameErrorCode.GameUserNotFound);
        }

        var currency = gameUser.Currency;
        if(currency == null)
        {
            Console.WriteLine($"Currency Not Found, User Id : {userId}");
            throw new ApiException(GameErrorCode.GameUserNotFound);
        }

        var currentGold = currency.Gold;
        if (currentGold != request.CurrentGold)
        {
            Console.WriteLine($"Gold value is not Equal, Account ID : {userId}, Server Gold : {currentGold}, Client Gold : {request.CurrentGold}");
            throw new ApiException(GameErrorCode.NotEqualData);
        }

        // TODO: 브롤러, 스킨, 아이템 등의 정보는 메모리에 올려놓고 사용하도록 변경 
        var brawler = await _context.Brawlers.FirstOrDefaultAsync(b => b.Id == request.CharacterId);
        if (brawler == null)
        {
            Console.WriteLine($"Brawler Not Found, brawler Id : {request.CharacterId}");
            throw new ApiException(GameErrorCode.NotFoundProduct);
        }

        if (gameUser.Brawlers.Any(b => b.BrawlerId == brawler.Id))
        {
            Console.WriteLine($"User Already Buy this Product, brawler Id : {brawler.Id}");
            throw new ApiException(GameErrorCode.AlreadyOwned);
        }

        if (currency.Gold < brawler.goldPrice)
        {
            Console.WriteLine($"User`s Gold is not Enough, Need : {brawler.goldPrice}, User`s Gold : {currency.Gold}");
            throw new ApiException(GameErrorCode.InsufficientCurrency);
        }

        currency.Gold -= brawler.goldPrice;
        UserBrawler newUserBrawler = UserBrawler.Create(gameUser, brawler);
        gameUser.Brawlers.Add(newUserBrawler);

        await _context.SaveChangesAsync();

        return new BuyCharacterResponse()
        {
            Brawler = UserBrawlerDto.Create(newUserBrawler),
            ResultGold = gameUser.Currency.Gold,
            Success = true
        };
    }

    public async Task<BuySkinResponse> BuySkin(int userId, BuySkinRequest request)
    {
        Console.WriteLine($"BuyCharacterRequest ID: {request.SkinId}, CurrentGold : {request.CurrentGem}");

        var gameUser = await _context.GameUsers
           .Include(u => u.Currency)
           .Include(u => u.Skins)
           .FirstOrDefaultAsync(u => u.AccountId == userId);

        if (gameUser == null)
        {
            Console.WriteLine($"GameUser Not Found, Account ID : {userId}");
            throw new ApiException(GameErrorCode.GameUserNotFound);
        }

        var currency = gameUser.Currency;
        if (currency == null)
        {
            Console.WriteLine($"Currency Not Found, User Id : {userId}");
            throw new ApiException(GameErrorCode.GameUserNotFound);
        }

        var currentGem = currency.Gems;
        if (currentGem != request.CurrentGem)
        {
            Console.WriteLine($"Gold value is not Equal, Account ID : {userId}, Server Gem : {currentGem}, Client Gem : {request.CurrentGem}");
            throw new ApiException(GameErrorCode.NotEqualData);
        }

        // TODO: 브롤러, 스킨, 아이템 등의 정보는 메모리에 올려놓고 사용하도록 변경 
        var skin = await _context.Skins.FirstOrDefaultAsync(b => b.Id == request.SkinId);
        if (skin == null)
        {
            Console.WriteLine($"Brawler Not Found, skin Id : {request.SkinId}");
            throw new ApiException(GameErrorCode.NotFoundProduct);
        }

        if (gameUser.Skins.Any(s => s.SkinId == skin.Id))
        {
            Console.WriteLine($"User Already Buy this Product, brawler Id : {skin.Id}");
            throw new ApiException(GameErrorCode.AlreadyOwned);
        }

        if (currency.Gold < skin.zemPrice)
        {
            Console.WriteLine($"User`s Gold is not Enough, Need : {skin.zemPrice}, User`s Gem : {currency.Gems}");
            throw new ApiException(GameErrorCode.InsufficientCurrency);
        }

        currency.Gems -= skin.zemPrice;
        UserSkin newUserSkin = UserSkin.Create(gameUser, skin);
        gameUser.Skins.Add(newUserSkin);

        await _context.SaveChangesAsync();
        return new BuySkinResponse()
        {
            ResultGem = gameUser.Currency.Gems,
            Success = true
        };
    }
}