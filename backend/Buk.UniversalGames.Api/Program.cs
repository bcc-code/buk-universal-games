using Buk.UniversalGames.Data;
using Buk.UniversalGames.Data.CacheRepositories;
using Buk.UniversalGames.Data.Interfaces;
using Buk.UniversalGames.Services;
using Buk.UniversalGames.Interfaces;
using Buk.UniversalGames.Library.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Buk.UniversalGames.Library.Enums;
using Buk.UniversalGames.Api.Authorization;

var builder = WebApplication.CreateBuilder(args);

var CorsPolicyName = "UBGCorsPolicy";

// Add services to the container.

builder.Services.AddRouting(options => options.LowercaseUrls = true);
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options => options.OperationFilter<TeamCodeHeaderFilter>());

builder.Services.AddScoped<ILeagueService, LeagueService>();
builder.Services.AddScoped<IGameService, GameService>();
builder.Services.AddScoped<IStickerService, StickerService>();
builder.Services.AddScoped<IStatusService, StatusService>();
builder.Services.AddScoped<ISettingsService, SettingsService>();

builder.Services.AddScoped<ILeagueRepository, LeagueCacheRepository>();
builder.Services.AddScoped<IGameRepository, GameCacheRepository>();
builder.Services.AddScoped<IStickerRepository, StickerCacheRepository>();
builder.Services.AddScoped<IStatusRepository, StatusCacheRepository>();
builder.Services.AddScoped<ISettingsRepository, SettingsCacheRepository>();

builder.Services.AddScoped<ICacheContext, CacheContext>();


builder.Services.AddDbContext<DataContext>(options =>
{
    var dbPort = Environment.GetEnvironmentVariable("POSTGRES_PORT");
    var dbName = Environment.GetEnvironmentVariable("POSTGRES_DB");
    var dbUser = Environment.GetEnvironmentVariable("POSTGRES_USER");
    var dbHost = Environment.GetEnvironmentVariable("POSTGRES_HOST");
    var dbPassword = Environment.GetEnvironmentVariable("POSTGRES_PASSWORD");
    var dataSourceBuilder = new Npgsql.NpgsqlDataSourceBuilder($"Host={dbHost}{(dbPort != "5432" ? ";Port=" + (dbPort ?? "") : "")};Database={dbName};Username={dbUser};Password={dbPassword};Pooling=false;Connection Idle Lifetime=0;Server Compatibility Mode=NoTypeLoading");

    options.UseNpgsql(dataSourceBuilder.Build());
});

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddDistributedMemoryCache();
    builder.Services.AddCors(options => options.AddPolicy(CorsPolicyName, policyBuilder =>
    {
        policyBuilder.AllowAnyOrigin()
                .WithHeaders("x-ubg-teamcode", "content-type")
              .WithMethods("GET", "POST", "OPTIONS").Build();
    }));
}
else
{
    builder.Services.AddStackExchangeRedisCache(options =>
    {
        options.Configuration = builder.Configuration.GetValue<string>("REDIS_CONNECTION_STRING");
        options.InstanceName = builder.Configuration.GetValue<string>("ENVIRONMENT_NAME");
    });
    builder.Services.AddCors(options => options.AddPolicy(CorsPolicyName, policyBuilder =>
    {
        policyBuilder.WithOrigins("https://universalgames.buk.no")
                .WithHeaders("x-ubg-teamcode", "content-type")
              .WithMethods("GET", "POST", "OPTIONS").Build();
    }));
}

// Ensure cookies work across all container instances
//var redis = ConnectionMultiplexer.Connect(builder.Configuration.GetValue<string>("REDIS_CONNECTION_STRING"));
// builder.Services.AddDataProtection().PersistKeysToStackExchangeRedis(redis, "wp-proxy-dataprotection-keys");

//builder.Services.AddMemoryCache();
builder.Services.AddApplicationInsightsTelemetry(options => { options.ConnectionString = builder.Configuration.GetValue<string>("APPLICATIONINSIGHTS_CONNECTION_STRING"); });


var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UsePathBase("/api");
    app.UseHttpsRedirection();
}

app.UseExceptionHandler(c => c.Run(async context =>
{
    var exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;
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

app.UseCors(CorsPolicyName);

app.UseStaticFiles();

app.UseSwagger();
app.UseSwaggerUI();

app.Map("~/", () => Results.Redirect("~/swagger"));

app.UseAuthorization();

app.MapControllers();

app.Run();
