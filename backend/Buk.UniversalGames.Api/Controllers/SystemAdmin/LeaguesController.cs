using Buk.UniversalGames.Api.Authorization;
using Buk.UniversalGames.Interfaces;
using Buk.UniversalGames.Library.Enums;
using Microsoft.AspNetCore.Mvc;

namespace Buk.UniversalGames.Api.Controllers.SystemAdmin;

[ApiController]
[TeamType(TeamType.SystemAdmin)]
[Route("{code}/SystemAdmin/[controller]")]
public class LeaguesController : ControllerBase
{
    private readonly ILogger<LeaguesController> _logger;
    private readonly ILeagueService _leagueService;

    public LeaguesController(ILogger<LeaguesController> logger, ILeagueService leagueService)
    {
        _logger = logger;
        _leagueService = leagueService;
    }

    [HttpGet]
    public IActionResult GetLeagues()
    {
        var xls = _leagueService.ExportTeams();
        return File(xls, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
    }

    [HttpGet("Status")]
    public IActionResult GetStatus()
    {
        var xls = _leagueService.ExportStatus();
        return File(xls, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
    }
}
