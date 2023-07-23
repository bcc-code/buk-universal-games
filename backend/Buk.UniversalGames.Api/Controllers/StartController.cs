using Buk.UniversalGames.Api.Exceptions;
using Buk.UniversalGames.Interfaces;
using Buk.UniversalGames.Library.Cultures;
using Buk.UniversalGames.Models;
using Microsoft.AspNetCore.Mvc;

namespace Buk.UniversalGames.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class StartController : ControllerBase
{
    private readonly ILogger<StartController> _logger;
    private readonly ILeagueService _leagueService;

    public StartController(ILogger<StartController> logger, ILeagueService leagueService)
    {
        _logger = logger;
        _leagueService = leagueService;
    }

    [HttpGet("{code}")]
    public async Task<ActionResult<SignInSuccessResponse>> Start(string code)
    {
        var team = await _leagueService.GetTeamByCode(code);
        if (team == null)
            return new ExceptionResult(Strings.UnknownTeamCode, 403);

        //var league = team.LeagueId.HasValue ? await _leagueService.GetLeague(team.LeagueId.Value) : null;

        return new SignInSuccessResponse(
            team.Code, 
            team.Name, 
            team.TeamType, 
            team.LeagueId, 
            team.LeagueId switch
            {
                4 => "B-League",
                5 => "U-League",
                6 => "K-League",
                _ => null
            });
    }
}
