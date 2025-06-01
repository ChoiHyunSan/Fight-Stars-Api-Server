public class GameUserInitializer : IGameUserInitializer
{
    private readonly AppDbContext _context;

    public GameUserInitializer(AppDbContext context)
    {
        _context = context;
    }

    public async Task InitializeNewUserAsync(long accountId, string nickname)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync();

        var user = new GameUser
        {
            AccountId = accountId,
            Nickname = nickname,
            CreatedAt = DateTime.UtcNow,
            LastLoginAt = DateTime.UtcNow
        };

        _context.GameUsers.Add(user);
        await _context.SaveChangesAsync(); // user.Id 확보

        _context.UserCurrencies.Add(new UserCurrency
        {
            GameUserId = user.Id,
            Gold = 500,
            Gems = 10,
            Energy = 5,
            Exp = 0
        });

        _context.UserStats.Add(new UserStats
        {
            GameUserId = user.Id,
            Level = 1,
            WinCount = 0,
            LoseCount = 0,
            TotalPlayCount = 0,
            HighestRank = 0,
            CurrentTrophy = 0
        });

        await _context.SaveChangesAsync();
        await transaction.CommitAsync();
    }

}
