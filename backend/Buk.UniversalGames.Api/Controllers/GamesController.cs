using Buk.UniversalGames.Api.Authorization;
using Buk.UniversalGames.Data.Models;
using Buk.UniversalGames.Data.Models.Internal;
using Buk.UniversalGames.Interfaces;
using Buk.UniversalGames.Library.Enums;
using Microsoft.AspNetCore.Mvc;

namespace Buk.UniversalGames.Api.Controllers;

[ApiController]
[Route("{code}/[controller]")]
public class GamesController : ControllerBase
{
    private readonly ILogger<GamesController> _logger;
    private readonly IGameService _gameService;

    public GamesController(ILogger<GamesController> logger, IGameService gameService)
    {
        _logger = logger;
        _gameService = gameService;
    }

    [TeamType(TeamType.Participant, TeamType.Admin, TeamType.SystemAdmin)]
    [HttpGet]
    public ActionResult<List<Game>> GetGames()
    {
        return _gameService.GetGames();
    }

    [TeamType(TeamType.Participant)]
    [HttpGet("Matches")]
    public ActionResult<List<MatchListItem>> GetMatches()
    {
        var team = HttpContext.Items["ValidatedTeam"] as Team;
        return _gameService.GetMatches(team);
    }
}
