using Buk.UniversalGames.Data;
using Buk.UniversalGames.Data.CacheRepositories;
using Buk.UniversalGames.Data.Interfaces;
using Buk.UniversalGames.Services;
using Buk.UniversalGames.Interfaces;
using Buk.UniversalGames.Library.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var UBGCorsPolicy = "UBGCorsPolicy";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: UBGCorsPolicy,
        policy =>
        {
            policy.WithOrigins("https://localhost:8080")
                  .WithMethods("GET", "POST", "OPTIONS");
        });
});

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<ILeagueService, LeagueService>();
builder.Services.AddScoped<IGameService, GameService>();
builder.Services.AddScoped<IStickerService, StickerService>();
builder.Services.AddScoped<IStatusService, StatusService>();

builder.Services.AddScoped<ILeagueRepository, LeagueCacheRepository>();
builder.Services.AddScoped<IGameRepository, GameCacheRepository>();
builder.Services.AddScoped<IStickerRepository, StickerCacheRepository>();
builder.Services.AddScoped<IStatusRepository, StatusCacheRepository>();

builder.Services.AddSingleton<ICacheContext, CacheContext>();

builder.Services.AddDbContext<DataContext>();

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddDistributedMemoryCache();
}
else
{
    builder.Services.AddStackExchangeRedisCache(options =>
    {
        options.Configuration = builder.Configuration.GetValue<string>("REDIS_CONNECTION_STRING");
        options.InstanceName = builder.Configuration.GetValue<string>("ENVIRONMENT_NAME");
    });
}

// Ensure cookies work across all container instances
//var redis = ConnectionMultiplexer.Connect(builder.Configuration.GetValue<string>("REDIS_CONNECTION_STRING"));
// builder.Services.AddDataProtection().PersistKeysToStackExchangeRedis(redis, "wp-proxy-dataprotection-keys");

builder.Services.AddMemoryCache();


var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UsePathBase("/api");
    app.UseHttpsRedirection();
}

app.UseExceptionHandler(c => c.Run(async context =>
{
    var exception = context.Features.Get<IExceptionHandlerFeature>().Error;
    if (exception is BadRequestException)
    {
        context.Response.StatusCode = 403;
        await context.Response.WriteAsJsonAsync(new
        {
            Error = exception.Message
        });
    }
}));

// Automatically migrate database
if (app.Environment.IsDevelopment())
{
    await Task.Delay(5000); //wait for DB to start up
}
var db = app.Services.CreateScope().ServiceProvider.GetService<DataContext>()!.Database;
await db.MigrateAsync();

app.UseRouting();

app.UseCors(UBGCorsPolicy);

app.UseStaticFiles();

app.UseSwagger();
app.UseSwaggerUI();

app.Map("~/", () => Results.Redirect("~/swagger"));

app.UseAuthorization();

app.MapControllers();

app.Run();
