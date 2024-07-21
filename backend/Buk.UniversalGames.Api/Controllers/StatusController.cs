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
using System.Globalization;
using Buk.UniversalGames.Library.Constants;
using System.Reflection.Metadata.Ecma335;

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
    private readonly ICacheContext _cacheContext;

    public StatusController(ILogger<StatusController> logger, StatusService statusService, ISettingsService settingsService, ValidatingCacheService validatingCacheService, IFamilyRepository familyRepository, ICacheContext cacheContext)
    {
        _logger = logger;
        _statusService = statusService;
        _settingsService = settingsService;
        _validatingCacheService = validatingCacheService;
        _familyRepository = familyRepository;
        _cacheContext = cacheContext;
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

            if (DateTime.TryParse(beginAndEnd[0], null, DateTimeStyles.AdjustToUniversal, out var hideHighScoreDate) && hideHighScoreDate < now
                && (beginAndEnd.Length < 2 || !DateTime.TryParse(beginAndEnd[1], null, DateTimeStyles.AdjustToUniversal, out var showAgainDate) || showAgainDate > now))
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
        var hideHighScore = await _validatingCacheService.WriteThrough("FrozenInterval", ()=>_settingsService.GetSetting("hide_highscore"));
        if (string.IsNullOrEmpty(hideHighScore))
        {
            throw new Exception("hide_highscore setting is required");
        }

        var now = DateTime.UtcNow;
        bool isFrozen = false;

        var beginAndEnd = hideHighScore.Split("|");

        if (!DateTime.TryParse(beginAndEnd[0], null, DateTimeStyles.AdjustToUniversal, out var hideHighScoreDate))
        {
            throw new Exception("Invalid hide_highscore start date format");
        }

        DateTime? showAgainDate = null;
        if (beginAndEnd.Length > 1 && DateTime.TryParse(beginAndEnd[1], null, DateTimeStyles.AdjustToUniversal, out var parsedShowAgainDate))
        {
            showAgainDate = parsedShowAgainDate;
        }

        if (hideHighScoreDate > now || (showAgainDate.HasValue && showAgainDate <= now))
        {
            isFrozen = false;
        }
        else
        {
            isFrozen = true;
        }

        FamilyStatusReport? report = null;

        if (isFrozen)
        {
            report = await _cacheContext.Get<FamilyStatusReport>(CacheKeys.FrozenCacheKey);
            if (report == null)
            {
                throw new Exception("Frozen leaderboard data is not available.");
            }
        }
        else
        {
            report = await _validatingCacheService.WriteThrough(CacheKeys.FamilyStatusCacheKey, _familyRepository.GetFamilyStatus);

            if (hideHighScoreDate > now)
            {
                await _cacheContext.Set(CacheKeys.FrozenCacheKey, report, 4500);
            }
        }

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
