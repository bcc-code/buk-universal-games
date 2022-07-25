using Buk.UniversalGames.Api.Authorization;
using Buk.UniversalGames.Data.Models;
using Buk.UniversalGames.Data.Models.Internal;
using Buk.UniversalGames.Interfaces;
using Buk.UniversalGames.Library.Enums;
using Buk.UniversalGames.Models;
using Microsoft.AspNetCore.Mvc;

namespace Buk.UniversalGames.Api.Controllers.Admin;

[ApiController]
[TeamType(TeamType.Admin,TeamType.SystemAdmin)]
[Route("{code}/Admin/[controller]")]
public class LeaguesController : ControllerBase
{
    private readonly ILogger<LeaguesController> _logger;
    private readonly ILeagueService _leagueService;
    private readonly IStatusService _statusService;
    private readonly IGameService _gameService;

    public LeaguesController(ILogger<LeaguesController> logger, ILeagueService leagueService , IStatusService statusService, IGameService gameService)
    {
        _logger = logger;
        _leagueService = leagueService;
        _statusService = statusService;
        _gameService = gameService;
    }

    [HttpGet]
    public async Task<ActionResult<List<League>>> GetLeagues()
    {
        return await _leagueService.GetLeagues();
    }

    [HttpGet("{leagueId}/Teams")]
    public async Task<ActionResult<List<Team>>> GetTeams(int leagueId)
    {
        return await _leagueService.GetTeams(leagueId);
    }

    [HttpGet("{leagueId}/Status")]
    public async Task<ActionResult<LeagueStatusReport>> GetLeagueStatus(int leagueId)
    {
        return await _statusService.GetLeagueStatus(leagueId);
    }

    [HttpGet("{leagueId}/Game/{gameId}/Matches")]
    public async Task<ActionResult<List<MatchListItem>>> GetGameMatches(int leagueId, int gameId)
    {
        return await _gameService.GetGameMatches(leagueId, gameId);
    }

    [HttpGet("{leagueId}/Matches")]
    public async Task<ActionResult<List<MatchListItem>>> GetAllMatches(int leagueId)
    {
        return await _gameService.GetGameMatches(leagueId);
    }
}
