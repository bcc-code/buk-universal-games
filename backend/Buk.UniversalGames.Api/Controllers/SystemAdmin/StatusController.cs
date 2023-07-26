using Buk.UniversalGames.Api.Authorization;
using Buk.UniversalGames.Data.Interfaces;
using Buk.UniversalGames.Interfaces;
using Buk.UniversalGames.Library.Enums;
using Microsoft.AspNetCore.Mvc;

namespace Buk.UniversalGames.Api.Controllers.SystemAdmin;

[ApiController]
[TeamType(TeamType.SystemAdmin)]
[Route("{code}/SystemAdmin/[controller]")]
public class StatusController : ControllerBase
{
    private readonly ILogger<StatusController> _logger;
    private readonly IStatusService _statusService;
    private readonly ILeagueRepository _leagueRepository;
    private readonly IGameRepository _gameRepository;

    public StatusController(ILogger<StatusController> logger, IStatusService statusService, ILeagueRepository leagueRepository, IGameRepository gameRepository)
    {
        _logger = logger;
        _statusService = statusService;
        _leagueRepository = leagueRepository;
        _gameRepository = gameRepository;
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
            foreach (var game in games)
                await _statusService.BuildAndCacheRankingForGameInLeague(game, league.LeagueId);

            await _statusService.BuildAndCacheRankingForSidequest(league.LeagueId);
            await _statusService.BuildAndCacheLeagueRanking(league.LeagueId);
        }
    }
}
