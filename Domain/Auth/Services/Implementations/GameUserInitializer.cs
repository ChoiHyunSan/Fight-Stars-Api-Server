public class GameUserInitializer : IGameUserInitializer
{
    private readonly AppDbContext _db;

    public GameUserInitializer(AppDbContext db)
    {
        _db = db;
    }

    public async Task InitializeNewUserAsync(long accountId, string nickname)
    {
        await using var transaction = await _db.Database.BeginTransactionAsync();

        var user = new GameUser
        {
            AccountId = accountId,
            Nickname = nickname,
            CreatedAt = DateTime.UtcNow,
            LastLoginAt = DateTime.UtcNow
        };

        _db.GameUsers.Add(user);
        await _db.SaveChangesAsync(); // user.Id 확보

        _db.UserCurrencies.Add(new UserCurrency
        {
            GameUserId = user.Id,
            Gold = 500,
            Gems = 10,
            Energy = 5,
            Exp = 0
        });

        _db.UserStats.Add(new UserStats
        {
            GameUserId = user.Id,
            Level = 1,
            WinCount = 0,
            LoseCount = 0,
            TotalPlayCount = 0,
            HighestRank = 0,
            CurrentTrophy = 0
        });

        await _db.SaveChangesAsync();
        await transaction.CommitAsync();
    }
}
