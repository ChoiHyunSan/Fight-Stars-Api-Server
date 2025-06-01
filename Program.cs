using FightStars_ApiServer.Gobal.Extensions;

var builder = WebApplication.CreateBuilder(args);

// 기본 서비스 등록
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// 확장 메서드로 서비스 등록
builder.Services
    .AddCustomSwagger()
    .AddCustomJwtAuthentication(builder.Configuration)
    .AddCustomDbContext(builder.Configuration)
    .AddCustomRedis(builder.Configuration)
    .AddCustomServices(builder.Configuration);

builder.Services.AddAuthorization();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "FightStars API v1");
    });
}

// 예외 처리 및 기타 미들웨어 등록
app.UseCustomMiddlewares();

// WebSocket 핸들러 등록
app.MapWebSocketHandlers();

// 앱 시작 전, 데이터 세팅
app.WebStartup();

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
