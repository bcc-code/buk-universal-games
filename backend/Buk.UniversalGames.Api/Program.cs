using Buk.UniversalGames.Data;
using Buk.UniversalGames.Data.CacheRepositories;
using Buk.UniversalGames.Data.Interfaces;
using Buk.UniversalGames.Services;
using Buk.UniversalGames.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<ILeagueService, LeagueService>();

builder.Services.AddScoped<ILeagueRepository, LeagueCacheRepository>();
builder.Services.AddScoped<ITeamRepository, TeamCacheRepository>();

builder.Services.AddDbContext<DataContext>();
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetValue<string>("REDIS_CONNECTION_STRING");
    options.InstanceName =  builder.Configuration.GetValue<string>("ENVIRONMENT_NAME");
});

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

// Automatically migrate database
if (app.Environment.IsDevelopment())
{
    await Task.Delay(5000); //wait for DB to start up
}
var db = app.Services.CreateScope().ServiceProvider.GetService<DataContext>()!.Database;
await db.MigrateAsync();

app.UseRouting();

app.UseStaticFiles();

app.UseSwagger();
app.UseSwaggerUI();

app.Map("~/", () => Results.Redirect("~/swagger"));

app.UseAuthorization();

app.MapControllers();

app.Run();
