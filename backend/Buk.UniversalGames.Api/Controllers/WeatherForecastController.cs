using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;

namespace Buk.UniversalGames.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;
    private readonly IDistributedCache _cache;

    public WeatherForecastController(ILogger<WeatherForecastController> logger, IDistributedCache cache)
    {
        _logger = logger;
        _cache = cache;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public async Task<IEnumerable<WeatherForecast>> Get()
    {
        var state = await _cache.GetStringAsync("WeatherState");
        if (state == null) {
            state = "1";
        } else {
            int iState = int.Parse(state);
            iState++;
            state = iState.ToString();
        }

        await _cache.SetStringAsync("WeatherState", state, new DistributedCacheEntryOptions {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(15)
        });

        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateTime.Now.AddDays(index),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)],
            CacheState = state
        })
        .ToArray();
    }
}
