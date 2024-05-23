using Buk.UniversalGames.Api.Authorization;
using Buk.UniversalGames.Api.Exceptions;
using Buk.UniversalGames.Data;
using Buk.UniversalGames.Data.Models;
using Buk.UniversalGames.Interfaces;
using Buk.UniversalGames.Library.Cultures;
using Buk.UniversalGames.Models;
using Buk.UniversalGames.Services;
using Microsoft.AspNetCore.Mvc;
using Buk.UniversalGames.Data.Models.Internal;
using Buk.UniversalGames.Data.Repositories;

namespace Buk.UniversalGames.Api.Controllers;

[ApiController]
[Participant]
[Route("[controller]")]
public class StatusController : ControllerBase
{
    private readonly ILogger<StatusController> _logger;
    private readonly StatusService _statusService;
    private readonly ISettingsService _settingsService;
    private readonly ValidatingCacheService _validatingCacheService;
    private readonly IFamilyRepository _familyRepository;


    public StatusController(ILogger<StatusController> logger, StatusService statusService, ISettingsService settingsService, ValidatingCacheService validatingCacheService, IFamilyRepository familyRepository)
    {
        _logger = logger;
        _statusService = statusService;
        _settingsService = settingsService;
        _validatingCacheService = validatingCacheService;
        _familyRepository = familyRepository;
    }


    [Obsolete("Deprecated")]
    [HttpGet]
    public async Task<ActionResult<TeamStatusReport>> Status()
    {
        var team = HttpContext.Items["ValidatedTeam"] as Team;
        return await _statusService.GetTeamStatus(team);
    }

    [HttpGet("League")]
    public async Task<ActionResult<LeagueStatusReport>> LeagueStatus()
    {
        var team = HttpContext.Items["ValidatedTeam"] as Team;

        if (!team!.LeagueId.HasValue)
            return new ExceptionResult(Strings.TeamNotPartOfALeague, 403);

        var hideHighScore = await _settingsService.GetSetting("hide_highscore");
        if (!string.IsNullOrEmpty(hideHighScore))
        {
            var now = DateTime.UtcNow;
            var beginAndEnd = hideHighScore.Split("|");

            if (DateTime.TryParse(beginAndEnd[0], out var hideHighScoreDate) && hideHighScoreDate < now
                && (beginAndEnd.Length < 2 || !DateTime.TryParse(beginAndEnd[1], out var showAgainDate) || showAgainDate > now))
            {
                return new ExceptionResult(Strings.HighScoreHidden, 406);
            }
        }

        return await _validatingCacheService.WriteThrough(LeagueStatusCacheKey(team.LeagueId.Value), () =>
        {
            return _statusService.GetLeagueStatus(team.LeagueId.Value);
        });
    }

    public static string LeagueStatusCacheKey(int leagueId)
    {
        return "LeagueStatus_leagueId_" + leagueId;
    }

    [HttpGet("Family")]
    public async Task<ActionResult<FamilyStatusReport>> FamilyStatus()
    {
        var hideHighScore = await _settingsService.GetSetting("hide_highscore");
        if (!string.IsNullOrEmpty(hideHighScore))
        {
            var now = DateTime.UtcNow;
            var beginAndEnd = hideHighScore.Split("|");

            if (DateTime.TryParse(beginAndEnd[0], out var hideHighScoreDate) && hideHighScoreDate < now
                && (beginAndEnd.Length < 2 || !DateTime.TryParse(beginAndEnd[1], out var showAgainDate) || showAgainDate > now))
            {
                return new ExceptionResult(Strings.HighScoreHidden, 406);
            }
        }

        return await _validatingCacheService.WriteThrough("FamilyStatusCacheKey", async () =>
        {
            return await _familyRepository.GetFamilyStatus();
        });
    }

}
