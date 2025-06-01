namespace FightStars_ApiServer.Gobal.Extensions
{
    public static class AppStartupExtensions
    {

        public static WebApplication WebStartup(this WebApplication app)
        {
            app.Lifetime.ApplicationStarted.Register(async () =>
            {
                using (var scope = app.Services.CreateScope())
                {
                    var shopService = scope.ServiceProvider.GetRequiredService<IShopService>();
                    await shopService.LoadShopDataToRedisAsync();
                }
            });
            return app; 
        }
    }
}
