using Microsoft.EntityFrameworkCore;

public class UserLoadDataService : IUserLoadDataService
{
    private readonly AppDbContext _db;

    public UserLoadDataService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<UserLoadDataResponse> LoadUserDataAsync(long userId)
    {
        var user = await _db.GameUsers
            .Include(u => u.Currency)
            .Include(u => u.Stats)
            .Include(u => u.Inventories)
            .Include(u => u.Brawlers)
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
            }).ToList()
        };
    }
}
