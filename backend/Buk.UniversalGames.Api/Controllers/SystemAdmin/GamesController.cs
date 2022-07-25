using Buk.UniversalGames.Api.Authorization;
using Buk.UniversalGames.Interfaces;
using Buk.UniversalGames.Library.Enums;
using Microsoft.AspNetCore.Mvc;

namespace Buk.UniversalGames.Api.Controllers.SystemAdmin;

[ApiController]
[TeamType(TeamType.SystemAdmin)]
[Route("{code}/SystemAdmin/[controller]")]
public class GamesController : ControllerBase
{
    private readonly ILogger<GamesController> _logger;
    private readonly IGameService _gameService;

    public GamesController(ILogger<GamesController> logger, IGameService gameService)
    {
        _logger = logger;
        _gameService = gameService;
    }

    [HttpGet("Matches")]
    public async Task<IActionResult> GetLeagues()
    {
        var xls = await _gameService.GetMatchExport();
        return File(xls, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
    }
}
