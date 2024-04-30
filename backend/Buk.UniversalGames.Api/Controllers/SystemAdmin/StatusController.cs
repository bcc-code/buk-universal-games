using Buk.UniversalGames.Api.Authorization;
using Buk.UniversalGames.Data;
using Buk.UniversalGames.Data.Interfaces;
using Buk.UniversalGames.Data.Models;
using Buk.UniversalGames.Interfaces;
using Buk.UniversalGames.Library.Enums;
using Buk.UniversalGames.Services;
using Microsoft.AspNetCore.Mvc;

namespace Buk.UniversalGames.Api.Controllers.SystemAdmin;

[ApiController]
[TeamType(TeamType.SystemAdmin)]
[Route("{code}/SystemAdmin/[controller]")]
public class StatusController : ControllerBase
{
    private readonly ILogger<StatusController> _logger;
    private readonly StatusService _statusService;
    private readonly ILeagueRepository _leagueRepository;
    private readonly IGameRepository _gameRepository;
    private readonly ICacheContext _cacheContext;

    public StatusController(ILogger<StatusController> logger, StatusService statusService, ILeagueRepository leagueRepository, IGameRepository gameRepository, ICacheContext cacheContext)
    {
        _logger = logger;
        _statusService = statusService;
        _leagueRepository = leagueRepository;
        _gameRepository = gameRepository;
        _cacheContext = cacheContext;
    }

    [HttpGet("ClearStatusAndMatches")]
    public async Task ClearStatusAndMatches()
    {
        await _statusService.ClearStatusAndMatches();
    }

    [HttpGet("UpdateRanking")]
    public async Task UpdateRanking()
    {
        var leagues = await _leagueRepository.GetLeagues();
        var games = await _gameRepository.GetGames();

        foreach (var league in leagues)
        {
            await _cacheContext.Remove($"Matches_{league.LeagueId}");
            await _cacheContext.Remove($"LeagueStatus_{league.LeagueId}");
            await _leagueRepository.GetTeams(league.LeagueId);
            await _gameRepository.GetMatches(league.LeagueId);

            foreach (var game in games)
                await _statusService.BuildAndCacheRankingForGameInLeague(game.Type, league.LeagueId);

            await _statusService.BuildAndCacheRankingForSidequest(league.LeagueId);
            await _statusService.BuildAndCacheLeagueRanking(league.LeagueId);


        }
    }

    [HttpGet("Export")]
    public async Task<IActionResult> GetStatus()
    {
        var xls = await _statusService.ExportStatus();
        return File(xls, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
    }
}
