using Domain.Game.Services;

namespace FightStars_ApiServer.Gobal.Extensions
{
    public static class MiddlewareExtensions
    {
        public static WebApplication UseCustomMiddlewares(this WebApplication app)
        {
            app.UseMiddleware<GlobalExceptionMiddleware>();
            return app;
        }

        public static WebApplication MapWebSocketHandlers(this WebApplication app)
        {
            app.UseWebSockets();
            app.Map("/ws/match", async context =>
            {
                if (context.WebSockets.IsWebSocketRequest)
                {
                    var socket = await context.WebSockets.AcceptWebSocketAsync();
                    var handler = context.RequestServices.GetRequiredService<MatchWebSocketHandler>();
                    await handler.HandleAsync(socket);
                }
                else
                {
                    context.Response.StatusCode = 400;
                }
            });
            return app;
        }
    }
}
