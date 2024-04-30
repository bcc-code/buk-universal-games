using Buk.UniversalGames.Api.Authorization;
using Buk.UniversalGames.Data.Models;
using Buk.UniversalGames.Data.Models.Matches;
using Buk.UniversalGames.Interfaces;
using Buk.UniversalGames.Library.Enums;
using Buk.UniversalGames.Models;
using Buk.UniversalGames.Services;
using Microsoft.AspNetCore.Mvc;

namespace Buk.UniversalGames.Api.Controllers.Admin;

[ApiController]
[TeamType(TeamType.Admin,TeamType.SystemAdmin)]
[Route("Admin/[controller]")]
public class LeaguesController : ControllerBase
{
    private readonly ILogger<LeaguesController> _logger;
    private readonly ILeagueService _leagueService;
    private readonly StatusService _statusService;
    private readonly GameService _gameService;

    public LeaguesController(ILogger<LeaguesController> logger, ILeagueService leagueService , StatusService statusService, GameService gameService)
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
