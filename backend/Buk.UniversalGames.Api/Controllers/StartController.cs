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

    private const string _chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

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
            },
            Enumerable.Range(0, 8).Select(x => GetCoins()),
            team.FamilyId.Value,
            team.Family.Name
            );
    }

    private static string GetCoins()
    {
        var stringChars = new char[8];
        var random = new Random();

        for (int i = 0; i < stringChars.Length; i++)
        {
            stringChars[i] = _chars[random.Next(_chars.Length)];
        }

        return new string(stringChars);
    }
}
