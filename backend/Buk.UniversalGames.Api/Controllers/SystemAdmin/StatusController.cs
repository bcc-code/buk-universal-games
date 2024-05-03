using Buk.UniversalGames.Api.Authorization;
using Buk.UniversalGames.Data;
using Buk.UniversalGames.Data.Interfaces;
using Buk.UniversalGames.Library.Enums;
using Buk.UniversalGames.Services;
using Microsoft.AspNetCore.Mvc;

namespace Buk.UniversalGames.Api.Controllers.SystemAdmin;

[ApiController]
[TeamType(TeamType.SystemAdmin)]
[Route("{code}/SystemAdmin/[controller]")]
public class StatusController : ControllerBase
{
    private readonly ILogger<StatusController> _logger;
    private readonly StatusService _statusService;
    private readonly ILeagueRepository _leagueRepository;
    private readonly IGameRepository _gameRepository;

    public StatusController(ILogger<StatusController> logger, StatusService statusService, ILeagueRepository leagueRepository, IGameRepository gameRepository)
    {
        _logger = logger;
        _statusService = statusService;
        _leagueRepository = leagueRepository;
        _gameRepository = gameRepository;
    }

    [HttpGet("ClearStatusAndMatches")]
    public async Task ClearStatusAndMatches()
    {
        await _statusService.ClearStatusAndMatches();
    }


    [HttpGet("Export")]
    public async Task<IActionResult> GetStatus()
    {
        var xls = await _statusService.ExportStatus();
        return File(xls, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
    }
}


