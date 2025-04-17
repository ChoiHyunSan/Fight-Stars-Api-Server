/***************************

   RefreshTokenCleanupJob

***************************/
// Description
// : 이미 사용했거나 만료된 RefreshToken을 정리하는 작업을 수행하는 클래스입니다.
//   이 작업은 매시간마다 실행되며, 만료된 RefreshToken을 데이터베이스에서 삭제합니다.
//
// Author : ChoiHyunSan
public class RefreshTokenCleanupJob : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<RefreshTokenCleanupJob> _logger;

    public RefreshTokenCleanupJob(IServiceScopeFactory scopeFactory, ILogger<RefreshTokenCleanupJob> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<IRefreshTokenService>();

                _logger.LogInformation("[RefreshTokenCleanupJob] 만료된 RefreshToken 정리 시작: {Time}", DateTime.UtcNow);
                await service.RemoveExpiredTokensAsync();
            }

            await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
        }
    }
}
