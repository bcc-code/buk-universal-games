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
[Route("{code}/[controller]")]
public class StatusController : ControllerBase
{
    private readonly ILogger<StatusController> _logger;
    private readonly IStatusService _statusService;

    public StatusController(ILogger<StatusController> logger, IStatusService statusService)
    {
        _logger = logger;
        _statusService = statusService;
    }

    [HttpGet]
    public ActionResult<TeamStatus> Status()
    {
        var team = HttpContext.Items["ValidatedTeam"] as Team;
        return _statusService.GetTeamStatus(team);
    }

    [HttpGet("League")]
    public ActionResult<List<TeamStatus>> LeagueStatus()
    {
        var team = HttpContext.Items["ValidatedTeam"] as Team;

        if(!team.LeagueId.HasValue)
            return new ExceptionResult(Strings.TeamNotPartOfALeague, 403);

        return _statusService.GetLeagueStatus(team.LeagueId.Value);
    }
}
