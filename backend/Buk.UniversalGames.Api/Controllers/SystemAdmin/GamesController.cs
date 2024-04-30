using Buk.UniversalGames.Api.Authorization;
using Buk.UniversalGames.Library.Enums;
using Buk.UniversalGames.Services;
using Microsoft.AspNetCore.Mvc;

namespace Buk.UniversalGames.Api.Controllers.SystemAdmin;

[ApiController]
[TeamType(TeamType.SystemAdmin)]
[Route("{code}/systemadmin/[controller]")]
public class GamesController : ControllerBase
{
    private readonly ILogger<GamesController> _logger;
    private readonly GameService _gameService;

    public GamesController(ILogger<GamesController> logger, GameService gameService)
    {
        _logger = logger;
        _gameService = gameService;
    }

    [HttpGet("matches")]
    public async Task<IActionResult> GetMatchReport()
    {
        var xls = await _gameService.GetMatchExport();
        return File(xls, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
    }
}
