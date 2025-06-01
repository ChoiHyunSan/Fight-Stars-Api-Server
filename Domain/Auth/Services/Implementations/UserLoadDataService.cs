using Microsoft.EntityFrameworkCore;

public class UserLoadDataService : IUserLoadDataService
{
    private readonly AppDbContext _context;

    public UserLoadDataService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<CreateGameResultResponse> CreateGameResultAsync(CreateGameResultRequest request)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync();

        CreateGameResultResponse response = new CreateGameResultResponse();

        foreach (var info in request.PlayerGameResults)
        {
            var user = await _context.GameUsers
                .Include(u => u.Stats)
                .Include(u => u.Currency)
                .FirstOrDefaultAsync(u => u.AccountId == info.PlayerId);
            if (user == null || user.Stats == null || user.Currency == null)
            {
                throw new ApiException(AuthErrorCodes.UserNotFound);
            }
            user.UpdateGameResult(info);

            UserGameResultData userGameResult = new UserGameResultData
            {
                UserId = user.AccountId,
                WinCount = user.Stats.WinCount,
                LoseCount = user.Stats.LoseCount,
                TotalPlayCount = user.Stats.TotalPlayCount,
                Gold = user.Currency.Gold,
                Energy = user.Currency.Energy,
                Exp = user.Currency.Exp,
                Level = user.Stats.Level
            };
            response.UserGameResults.Add(userGameResult);
        }

        await _context.SaveChangesAsync();
        await transaction.CommitAsync();

        return response;
    }

    public async Task<UserLoadDataResponse> LoadUserDataAsync(long userId)
    {
        var user = await _context.GameUsers
            .Include(u => u.Currency)
            .Include(u => u.Stats)
            .Include(u => u.Inventories)
            .Include(u => u.Brawlers)
            .Include(u => u.Skins)
            .FirstOrDefaultAsync(u => u.AccountId == userId);

        if (user == null)
        {
            throw new ApiException(AuthErrorCodes.UserNotFound);
        }

        return new UserLoadDataResponse
        {
            UserId = user.AccountId,
            Nickname = user.Nickname,
            Avatar = user.Avatar,
            Currency = new UserCurrencyDto
            {
                Gold = user.Currency?.Gold ?? 0,
                Gems = user.Currency?.Gems ?? 0,
                Energy = user.Currency?.Energy ?? 0,
                Exp = user.Currency?.Exp ?? 0
            },
            Stats = new UserStatsDto
            {
                Level = user.Stats?.Level ?? 1,
                WinCount = user.Stats?.WinCount ?? 0,
                LoseCount = user.Stats?.LoseCount ?? 0,
                TotalPlayCount = user.Stats?.TotalPlayCount ?? 0,
                HighestRank = user.Stats?.HighestRank ?? 0,
                CurrentTrophy = user.Stats?.CurrentTrophy ?? 0
            },
            Inventory = user.Inventories.Select(i => new UserInventoryDto
            {
                ItemId = i.ItemId,
                Quantity = i.Quantity
            }).ToList(),
            Brawlers = user.Brawlers.Select(b => new UserBrawlerDto
            {
                BrawlerId = b.BrawlerId,
                Level = b.Level,
                Trophy = b.Trophy,
                PowerPoint = b.PowerPoint
            }).ToList(),
            Skins = new UserSkinDto
            {
                SkinIds = user.Skins.Select(s => s.SkinId).ToList()
            }
        };
    }
}
