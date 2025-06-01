
using FightStars_ApiServer.Gobal.Utils;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using System.Text.Json;
using System.Text.Json.Serialization;

public class ShopService : IShopService
{
    private readonly AppDbContext _context;
    private readonly IConnectionMultiplexer _redis;

    public ShopService(
        AppDbContext context, 
        IConnectionMultiplexer redis)
    {
        _context = context;
        _redis = redis;
    }

    private static readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };

    public async Task LoadShopDataToRedisAsync()
    {
        await LoadBrawlersToRedisAsync();
        await LoadSkinsToRedisAsync();
    }

    private async Task LoadBrawlersToRedisAsync()
    {
        var brawlers = await _context.Brawlers.ToListAsync();
        var db = _redis.GetDatabase();

        var tasks = brawlers.Select(brawler =>
        {
            string key = RedisKeys.Brawler(brawler.Id);
            string json = JsonSerializer.Serialize(brawler, _jsonOptions);
            return db.StringSetAsync(key, json);
        });

        await Task.WhenAll(tasks);
        Console.WriteLine($"Loaded {brawlers.Count} brawlers into Redis.");
    }

    private async Task LoadSkinsToRedisAsync()
    {
        var skins = await _context.Skins.ToListAsync();
        var db = _redis.GetDatabase();

        var tasks = skins.Select(skin =>
        {
            string key = RedisKeys.Skin(skin.Id);
            string json = JsonSerializer.Serialize(skin, _jsonOptions);
            return db.StringSetAsync(key, json);
        });

        await Task.WhenAll(tasks);
        Console.WriteLine($"Loaded {skins.Count} skins into Redis.");
    }

    public async Task<BuyCharacterResponse> BuyCharacter(int userId, BuyCharacterRequest request)
    {
        Console.WriteLine($"BuyCharacterRequest ID: {request.CharacterId}, CurrentGold : {request.CurrentGold}");

        var gameUser = await _context.GameUsers
            .Include(u => u.Currency)
            .Include(u => u.Brawlers)
            .FirstOrDefaultAsync(u => u.AccountId == userId);

        if (gameUser == null || gameUser.Currency == null)
        {
            Console.WriteLine($"GameUser or Currency Not Found, Account ID : {userId}");
            throw new ApiException(GameErrorCode.GameUserNotFound);
        }

        if (gameUser.Currency.Gold != request.CurrentGold)
        {
            Console.WriteLine($"Gold mismatch. Server: {gameUser.Currency.Gold}, Client: {request.CurrentGold}");
            throw new ApiException(GameErrorCode.NotEqualData);
        }

        var db = _redis.GetDatabase();
        string redisKey = RedisKeys.Brawler(request.CharacterId);
        var brawlerJson = await db.StringGetAsync(redisKey);

        Brawler? brawler = !brawlerJson.IsNullOrEmpty
            ? JsonSerializer.Deserialize<Brawler>(brawlerJson!, _jsonOptions)
            : await _context.Brawlers.FirstOrDefaultAsync(b => b.Id == request.CharacterId);

        if (brawler == null)
        {
            Console.WriteLine($"Brawler Not Found, Id: {request.CharacterId}");
            throw new ApiException(GameErrorCode.NotFoundProduct);
        }

        if (gameUser.Brawlers.Any(b => b.BrawlerId == brawler.Id))
            throw new ApiException(GameErrorCode.AlreadyOwned);

        if (gameUser.Currency.Gold < brawler.goldPrice)
            throw new ApiException(GameErrorCode.InsufficientCurrency);

        gameUser.Currency.Gold -= brawler.goldPrice;
        gameUser.Brawlers.Add(UserBrawler.Create(gameUser, brawler));

        await _context.SaveChangesAsync();

        return new BuyCharacterResponse
        {
            Brawler = UserBrawlerDto.Create(gameUser.Brawlers.Last()),
            ResultGold = gameUser.Currency.Gold,
            Success = true
        };
    }

    public async Task<BuySkinResponse> BuySkin(int userId, BuySkinRequest request)
    {
        Console.WriteLine($"BuySkinRequest ID: {request.SkinId}, CurrentGem : {request.CurrentGem}");

        var gameUser = await _context.GameUsers
            .Include(u => u.Currency)
            .Include(u => u.Skins)
            .FirstOrDefaultAsync(u => u.AccountId == userId);

        if (gameUser == null || gameUser.Currency == null)
        {
            Console.WriteLine($"GameUser or Currency Not Found, Account ID : {userId}");
            throw new ApiException(GameErrorCode.GameUserNotFound);
        }

        if (gameUser.Currency.Gems != request.CurrentGem)
        {
            Console.WriteLine($"Gem mismatch. Server: {gameUser.Currency.Gems}, Client: {request.CurrentGem}");
            throw new ApiException(GameErrorCode.NotEqualData);
        }

        var db = _redis.GetDatabase();
        string redisKey = RedisKeys.Skin(request.SkinId);
        var skinJson = await db.StringGetAsync(redisKey);

        Skin? skin = !skinJson.IsNullOrEmpty
            ? JsonSerializer.Deserialize<Skin>(skinJson!, _jsonOptions)
            : await _context.Skins.FirstOrDefaultAsync(s => s.Id == request.SkinId);

        if (skin == null)
        {
            Console.WriteLine($"Skin Not Found, Id: {request.SkinId}");
            throw new ApiException(GameErrorCode.NotFoundProduct);
        }

        if (gameUser.Skins.Any(s => s.SkinId == skin.Id))
            throw new ApiException(GameErrorCode.AlreadyOwned);

        if (gameUser.Currency.Gems < skin.zemPrice)
            throw new ApiException(GameErrorCode.InsufficientCurrency);

        gameUser.Currency.Gems -= skin.zemPrice;
        gameUser.Skins.Add(UserSkin.Create(gameUser, skin));

        await _context.SaveChangesAsync();

        return new BuySkinResponse
        {
            ResultGem = gameUser.Currency.Gems,
            Success = true
        };
    }
}