using Buk.UniversalGames.Api.Authorization;
using Buk.UniversalGames.Data;
using Buk.UniversalGames.Data.Interfaces;
using Buk.UniversalGames.Library.Enums;
using Buk.UniversalGames.Services;
using Microsoft.AspNetCore.Mvc;

namespace Buk.UniversalGames.Api.Controllers.SystemAdmin;

[ApiController]
[TeamType(TeamType.SystemAdmin)]
[Route("{code}/[controller]")]
[ResponseCache(NoStore = true)]
public class CacheController : ControllerBase
{
    private readonly ILogger<CacheController> _logger;
    private readonly ICacheContext _cache;
    private readonly ILeagueRepository _leagueRepository;
    private readonly IGameRepository _gameRepository;
    private readonly StatusService _statusService;

    public CacheController(ILogger<CacheController> logger, StatusService statusService, ILeagueRepository leagueRepository, IGameRepository gameRepository, ICacheContext cache)
    {
        _logger = logger;
        _cache = cache;
        _leagueRepository = leagueRepository;
        _gameRepository = gameRepository;
        _statusService = statusService;
    }

    [HttpGet("clear")]
    public async Task<IActionResult> ClearCache()
    {
        await _cache.Clear();
        return Ok();
    }

    [HttpGet("warmup")]
    public async Task<IActionResult> PreCache()
    {
        var leagues = await _leagueRepository.GetLeagues();
        var games = await _gameRepository.GetGames();
        await _statusService.GuaranteeAnswersInCache();

        foreach (var league in leagues)
        {
            await _cache.Remove($"Matches_{league.LeagueId}");
            await _cache.Remove($"LeagueStatus_{league.LeagueId}");
            await _leagueRepository.GetTeams(league.LeagueId);
            await _gameRepository.GetMatches(league.LeagueId);

            foreach (var game in games)
                await _statusService.BuildAndCacheRankingForGameInLeague(game, league.LeagueId);
            await _statusService.BuildAndCacheLeagueRanking(league.LeagueId);

        }
        return Ok();
    }
}
