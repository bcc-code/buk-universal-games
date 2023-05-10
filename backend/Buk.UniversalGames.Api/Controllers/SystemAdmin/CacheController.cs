using Buk.UniversalGames.Api.Authorization;
using Buk.UniversalGames.Data;
using Buk.UniversalGames.Data.Interfaces;
using Buk.UniversalGames.Library.Enums;
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
    private readonly IStickerRepository _stickerRepository;
    private readonly IStatusRepository _statusRepository;

    public CacheController(ILogger<CacheController> logger, IStatusRepository statusRepository, ILeagueRepository leagueRepository, IGameRepository gameRepository, IStickerRepository stickerRepository, ICacheContext cache)
    {
        _logger = logger;
        _cache = cache;
        _leagueRepository = leagueRepository;
        _gameRepository = gameRepository;
        _stickerRepository = stickerRepository;
        _statusRepository = statusRepository;
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

        foreach (var league in leagues)
        {
            await _leagueRepository.GetTeams(league.LeagueId);
            await _gameRepository.GetMatches(league.LeagueId);
            await _stickerRepository.GetStickers(league.LeagueId);
            await _statusRepository.GetLeagueStatus(league.LeagueId);
        }
        return Ok();
    }
}
