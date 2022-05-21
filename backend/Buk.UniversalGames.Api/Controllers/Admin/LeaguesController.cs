using Buk.UniversalGames.Api.Authorization;
using Buk.UniversalGames.Data.Models;
using Buk.UniversalGames.Interfaces;
using Buk.UniversalGames.Library.Enums;
using Microsoft.AspNetCore.Mvc;

namespace Buk.UniversalGames.Api.Controllers.Admin;

[ApiController]
[TeamType(TeamType.Admin,TeamType.SystemAdmin)]
[Route("{code}/Admin/[controller]")]
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
    public IEnumerable<League> All()
    {
        return _leagueService.GetLeagues();
    }

    [HttpGet("{leagueId}/Teams")]
    public IEnumerable<Team> GetTeams(int leagueId)
    {
        return _leagueService.GetTeams(leagueId);
    }
}
