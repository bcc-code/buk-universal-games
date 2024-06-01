using Buk.UniversalGames.Api.Authorization;
using Buk.UniversalGames.Data.Models;
using Buk.UniversalGames.Data.Models.Internal;
using Buk.UniversalGames.Data.Models.Matches;
using Buk.UniversalGames.Library.Enums;
using Buk.UniversalGames.Services;
using Microsoft.AspNetCore.Mvc;

namespace Buk.UniversalGames.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class GamesController : ControllerBase
{
    private readonly ILogger<GamesController> _logger;
    private readonly GameService _gameService;
    private readonly StatusService _statusService;

    public GamesController(ILogger<GamesController> logger, GameService gameService, StatusService statusService)
    {
        _logger = logger;
        _gameService = gameService;
        _statusService = statusService;
    }

    [TeamType(TeamType.Participant, TeamType.Admin, TeamType.SystemAdmin)]
    [HttpGet]
    public async Task<ActionResult<List<Game>>> GetGames()
    {
        // shit this should be cached
        var games = await _gameService.GetGames();
        return games;
    }

    [TeamType(TeamType.Participant, TeamType.Admin, TeamType.SystemAdmin)]
    [HttpGet("{gameId}/ranking/{leagueId}")]
    public async Task<ActionResult<List<TeamStatus>>> GetGameRanking(int gameId, int leagueId)
    {
        var games = await _gameService.GetGames();
        return await _statusService.GetGameRanking(games.First(x => x.GameId == gameId).Type, leagueId);
    }

    [TeamType(TeamType.Participant)]
    [HttpGet("/matches")]
    public async Task<ActionResult<List<MatchListItem>>> GetMatches()
    {
        var team = HttpContext.Items["ValidatedTeam"] as Team;
        // shit this should be cached.
        return await _gameService.GetMatches(team);
    }
}
