using Buk.UniversalGames.Api.Authorization;
using Buk.UniversalGames.Data;
using Buk.UniversalGames.Data.Interfaces;
using Buk.UniversalGames.Data.Models.Matches;
using Buk.UniversalGames.Library.Enums;
using Buk.UniversalGames.Services;
using Microsoft.AspNetCore.Mvc;

namespace Buk.UniversalGames.Api.Controllers.Admin;

[ApiController]
[TeamType(TeamType.Admin, TeamType.SystemAdmin)]
public class GamesController : ControllerBase
{
    private readonly ILogger<GamesController> _logger;
    private readonly GameService _gameService;
    private readonly ValidatingCacheService _validatingCacheService;
    private readonly IGameRepository _gameRepository;

    public GamesController(ILogger<GamesController> logger, GameService gameService, ValidatingCacheService validatingCacheService, IGameRepository gameRepository)
    {
        _logger = logger;
        _gameService = gameService;
        _validatingCacheService = validatingCacheService;
        _gameRepository = gameRepository;
    }


    [HttpPost("matches/{matchId}/results")]
    public async Task<ActionResult<MatchListItem>> PostMatchResult([FromBody] MatchResultDto matchResult)
    {
        var teamMatchResult = await _gameService.ReportTeamMatchResult(matchResult.MatchId, matchResult.TeamId, matchResult.Result);
        var match = await _gameRepository.GetMatch(matchResult.MatchId);
        var leagueId = match.LeagueId;

        await _validatingCacheService.Remove(StatusController.LeagueStatusCacheKey(leagueId));
        await _validatingCacheService.Remove(StatusController.FamilyStatusCacheKey);

        return teamMatchResult;
    }

    [HttpPost("matches/bothresults")]
    public async Task<ActionResult<MatchListItem>> PostMatchResults([FromBody] MatchResultsDto matchResults)
    {
        var matchResult = await _gameService.ReportTeamMatchResults(matchResults.MatchId, matchResults.Team1Result, matchResults.Team2Result);
        var match = await _gameRepository.GetMatch(matchResults.MatchId);
        var leagueId = match.LeagueId;

        await _validatingCacheService.Remove(StatusController.LeagueStatusCacheKey(leagueId));
        await _validatingCacheService.Remove(StatusController.FamilyStatusCacheKey);

        return matchResult;
    }
}
