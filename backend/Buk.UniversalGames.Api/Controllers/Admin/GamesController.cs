using Buk.UniversalGames.Api.Authorization;
using Buk.UniversalGames.Data;
using Buk.UniversalGames.Data.Models;
using Buk.UniversalGames.Data.Models.Matches;
using Buk.UniversalGames.Interfaces;
using Buk.UniversalGames.Library.Enums;
using Microsoft.AspNetCore.Mvc;

namespace Buk.UniversalGames.Api.Controllers.Admin;

[ApiController]
[TeamType(TeamType.Admin,TeamType.SystemAdmin)]
public class GamesController : ControllerBase
{
    private readonly ILogger<GamesController> _logger;
    private readonly IGameService _gameService;

    public GamesController(ILogger<GamesController> logger, IGameService gameService)
    {
        _logger = logger;
        _gameService = gameService;
    }


    [HttpPost("matches/{matchId}/results")]
    public async Task<ActionResult<MatchListItem>> PostMatchResult([FromBody]MatchResultDto matchResult)
    {
        return await _gameService.ReportTeamMatchResult(matchResult.MatchId, matchResult.TeamId, matchResult.Result);
    }

    [HttpPost("matches/{matchId}/Winner/{teamId}")]
    public async Task<ActionResult<MatchWinnerResult>> SetMatchWinner(int matchId, int teamId)
    {
        return await _gameService.SetMatchWinner(matchId, teamId);
    }
}
