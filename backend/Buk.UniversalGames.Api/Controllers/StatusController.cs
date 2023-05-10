using Buk.UniversalGames.Api.Authorization;
using Buk.UniversalGames.Api.Exceptions;
using Buk.UniversalGames.Data.Models;
using Buk.UniversalGames.Interfaces;
using Buk.UniversalGames.Library.Cultures;
using Buk.UniversalGames.Models;
using Microsoft.AspNetCore.Mvc;

namespace Buk.UniversalGames.Api.Controllers;

[ApiController]
[Participant]
[Route("[controller]")]
public class StatusController : ControllerBase
{
    private readonly ILogger<StatusController> _logger;
    private readonly IStatusService _statusService;
    private readonly ISettingsService _settingsService;

    public StatusController(ILogger<StatusController> logger, IStatusService statusService, ISettingsService settingsService)
    {
        _logger = logger;
        _statusService = statusService;
        _settingsService = settingsService;
    }

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

        if(!team!.LeagueId.HasValue)
            return new ExceptionResult(Strings.TeamNotPartOfALeague, 403);

        var hideHighScore = await _settingsService.GetSetting("hide_highscore");
        if (hideHighScore != null)
        {
            DateTime.TryParse(hideHighScore, out var hideHighScoreDate);
            if (hideHighScoreDate != DateTime.MinValue && hideHighScoreDate < DateTime.Now)
                return new ExceptionResult(Strings.HighScoreHidden, 406);
        }

        return await _statusService.GetLeagueStatus(team.LeagueId.Value);
    }
}
