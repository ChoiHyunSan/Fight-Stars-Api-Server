// Program.cs
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Text;
using Microsoft.Extensions.Options;
using StackExchange.Redis;
using Domain.Game.Services;

var builder = WebApplication.CreateBuilder(args);

// 1. 컨트롤러 및 Swagger 설정
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "FightStars API",
        Version = "v1",
        Description = "API documentation for FightStars"
    });

    // 🔐 JWT 인증 Swagger 설정 (보안 설정이 있을 경우)
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme. Example: 'Bearer {token}'"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// 2. JWT 인증 설정
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)
            ),
            ClockSkew = TimeSpan.Zero
        };

        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                {
                    context.Response.Headers.Add("Token-Expired", "true");
                }
                return Task.CompletedTask;
            }
        };
    });

// 3. DB 연결 설정 (MySQL 예시)
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        new MySqlServerVersion(new Version(8, 0, 35))
    );
});

// Redis 설정 추가
builder.Services.Configure<RedisConfig>(
    builder.Configuration.GetSection("Redis"));

builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
{
    var config = sp.GetRequiredService<IOptions<RedisConfig>>().Value;

    var options = new ConfigurationOptions
    {
        EndPoints = { $"{config.Host}:{config.Port}" },
        Password = config.Password,
        AbortOnConnectFail = false,
        ConnectRetry = 3
    };

    return ConnectionMultiplexer.Connect(options);
});

// 4. 의존성 주입 (DI)
builder.Services.AddAuthorization();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IRefreshTokenService, RefreshTokenService>();
builder.Services.AddHostedService<RefreshTokenCleanupJob>();
builder.Services.AddScoped<IGameUserInitializer, GameUserInitializer>();
builder.Services.AddScoped<IUserLoadDataService, UserLoadDataService>();
builder.Services.AddSingleton<MatchWebSocketHandler>();
builder.Services.AddSingleton<MatchManager>();
builder.Services.AddSingleton<RoomDispatcher>();

var app = builder.Build();

// 5. 미들웨어 구성 (순서 중요)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "FightStars API v1");
    });
}

app.UseMiddleware<GlobalExceptionMiddleware>(); // 예외 처리 미들웨어는 가장 위에

app.UseWebSockets(); // 반드시 먼저 등록

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

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();