using Buk.UniversalGames.Api.Authorization;
using Buk.UniversalGames.Data;
using Buk.UniversalGames.Data.Interfaces;
using Buk.UniversalGames.Data.Models.Matches;
using Buk.UniversalGames.Library.Constants;
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
    private readonly ICacheContext _cache;

    public GamesController(ILogger<GamesController> logger, GameService gameService, ValidatingCacheService validatingCacheService, IGameRepository gameRepository, ICacheContext cache)
    {
        _logger = logger;
        _gameService = gameService;
        _validatingCacheService = validatingCacheService;
        _gameRepository = gameRepository;
        _cache = cache;
    }


    [HttpPost("matches/{matchId}/results")]
    public async Task<ActionResult<MatchListItem>> PostMatchResult([FromBody] MatchResultDto matchResult)
    {
        var teamMatchResult = await _gameService.ReportTeamMatchResult(matchResult.MatchId, matchResult.TeamId, matchResult.Result);

await _cache.Clear();

        return teamMatchResult;
    }

    [HttpPost("matches/bothresults")]
    public async Task<ActionResult<MatchListItem>> PostMatchResults([FromBody] MatchResultsDto matchResults)
    {
        var matchResult = await _gameService.ReportTeamMatchResults(matchResults.MatchId, matchResults.Team1Result, matchResults.Team2Result);
await _cache.Clear();
        return matchResult;
    }
}
