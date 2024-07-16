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
using Buk.UniversalGames.Library.Enums;

namespace Buk.UniversalGames.Api.Controllers;

[ApiController]
[TeamType(TeamType.Admin, TeamType.SystemAdmin, TeamType.Participant)]
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
        var now = DateTime.UtcNow;
        bool isFrozen = false;

        if (!string.IsNullOrEmpty(hideHighScore))
        {
            var beginAndEnd = hideHighScore.Split("|");

            if (DateTime.TryParse(beginAndEnd[0], null, System.Globalization.DateTimeStyles.AdjustToUniversal, out var hideHighScoreDate))
            {
                _logger.LogInformation($"HideHighScoreDate (begin): {hideHighScoreDate}");
                _logger.LogInformation($"Now: {now}");
                _logger.LogInformation($"Is hideHighScoreDate < now: {hideHighScoreDate < now}");
            }

            if (beginAndEnd.Length < 2)
            {
                isFrozen = hideHighScoreDate < now;
            }
            else if (DateTime.TryParse(beginAndEnd[1], null, System.Globalization.DateTimeStyles.AdjustToUniversal, out var showAgainDate))
            {
                _logger.LogInformation($"ShowAgainDate (end): {showAgainDate}");
                _logger.LogInformation($"Now: {now}");
                _logger.LogInformation($"Is showAgainDate > now: {showAgainDate > now}");

                isFrozen = hideHighScoreDate < now && showAgainDate > now;
            }
        }

        FamilyStatusReport report = await _validatingCacheService.WriteThrough("FamilyStatusCacheKey", async () =>
        {
            return await _familyRepository.GetFamilyStatus();
        });

        var team = HttpContext.Items["ValidatedTeam"] as Team;
        if (team != null)
        {
            var currentTeamStatus = report.Families
                .SelectMany(f => f.Teams)
                .FirstOrDefault(t => t.TeamId == team.TeamId);

            var currentFamilyStatus = report.Families
                .FirstOrDefault(f => f.Id == team.FamilyId);

            report.MyStatus.TeamPoints = currentTeamStatus?.Points ?? 0;
            report.MyStatus.FamilyPoints = currentFamilyStatus?.Points ?? 0;
        }

        report.IsFrozen = isFrozen;

        return report;
    }
}