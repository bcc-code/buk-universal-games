using Buk.UniversalGames.Api.Authorization;
using Buk.UniversalGames.Data;
using Buk.UniversalGames.Data.Interfaces;
using Buk.UniversalGames.Interfaces;
using Buk.UniversalGames.Library.Enums;
using Microsoft.AspNetCore.Mvc;

namespace Buk.UniversalGames.Api.Controllers.SystemAdmin;

[ApiController]
[TeamType(TeamType.SystemAdmin)]
[Route("{code}/SystemAdmin/[controller]")]
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

    [HttpGet("ClearCache")]
    public IActionResult ClearCache()
    {
        _cache.Clear();
        return Ok();
    }

    [HttpGet("PreCache")]
    public IActionResult PreCache()
    {
        var leagues = _leagueRepository.GetLeagues();
        var games = _gameRepository.GetGames();

        foreach (var league in leagues)
        {
            _leagueRepository.GetTeams(league.LeagueId);
            _gameRepository.GetGameMatches(league.LeagueId);
            _stickerRepository.GetStickers(league.LeagueId);
            _statusRepository.GetLeagueStatus(league.LeagueId);
        }
        return Ok();
    }
}
