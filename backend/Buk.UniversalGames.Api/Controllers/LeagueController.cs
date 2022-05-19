using Buk.UniversalGames.Data.Models;
using Buk.UniversalGames.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Buk.UniversalGames.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class LeagueController : ControllerBase
{
    private readonly ILogger<LeagueController> _logger;
    private readonly ILeagueService _leagueService;

    public LeagueController(ILogger<LeagueController> logger, ILeagueService leagueService)
    {
        _logger = logger;
        _leagueService = leagueService;
    }

    [HttpGet("Leagues")]
    public IEnumerable<League> Get()
    {
        return _leagueService.GetLeagues();
    }

    [HttpGet("Teams/{leagueId}")]
    public IEnumerable<Team> GetTeams(int leagueId)
    {
        return _leagueService.GetTeams(leagueId);
    }
}
