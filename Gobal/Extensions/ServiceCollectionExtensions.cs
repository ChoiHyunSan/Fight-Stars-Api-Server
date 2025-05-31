using Domain.Game.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using StackExchange.Redis;
using System.Text;

namespace FightStars_ApiServer.Gobal.Extensions
{
    public static class ServiceCollectionExtensions
    {
        
        // 인터페이스를 통해 서비스들을 등록
        public static IServiceCollection AddCustomServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IJwtService, JwtService>();
            services.AddScoped<IRefreshTokenService, RefreshTokenService>();
            services.AddHostedService<RefreshTokenCleanupJob>();
            services.AddScoped<IGameUserInitializer, GameUserInitializer>();
            services.AddScoped<IUserLoadDataService, UserLoadDataService>();
            services.AddSingleton<MatchWebSocketHandler>();
            services.AddSingleton<MatchManager>();
            services.AddSingleton<RoomDispatcher>();
            services.AddScoped<IGameDataService, GameDataService>();
            services.AddScoped<IShopService, ShopService>();
            return services;
        }

        // JWT 인증 설정
        public static IServiceCollection AddCustomJwtAuthentication(this IServiceCollection services, IConfiguration config)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = config["Jwt:Issuer"],
                        ValidAudience = config["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]!)),
                        ClockSkew = TimeSpan.Zero
                    };
                    options.Events = new JwtBearerEvents
                    {
                        OnAuthenticationFailed = context =>
                        {
                            if (context.Exception is SecurityTokenExpiredException)
                            {
                                context.Response.Headers.Add("Token-Expired", "true");
                            }
                            return Task.CompletedTask;
                        }
                    };
                });
            return services;
        }

        // MySQL 데이터베이스 연결 설정
        public static IServiceCollection AddCustomDbContext(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseMySql(
                    config.GetConnectionString("DefaultConnection"),
                    new MySqlServerVersion(new Version(8, 0, 35))
                ));
            return services;
        }

        // Redis 연결 설정
        public static IServiceCollection AddCustomRedis(this IServiceCollection services, IConfiguration config)
        {
            services.Configure<RedisConfig>(config.GetSection("Redis"));
            services.AddSingleton<IConnectionMultiplexer>(sp =>
            {
                var redisConfig = sp.GetRequiredService<IOptions<RedisConfig>>().Value;
                var options = new ConfigurationOptions
                {
                    EndPoints = { $"{redisConfig.Host}:{redisConfig.Port}" },
                    Password = redisConfig.Password,
                    AbortOnConnectFail = false,
                    ConnectRetry = 3
                };
                return ConnectionMultiplexer.Connect(options);
            });
            return services;
        }
    }
}

