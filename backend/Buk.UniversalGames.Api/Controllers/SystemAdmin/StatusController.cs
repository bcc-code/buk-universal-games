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
    private readonly ICacheContext _cache;

    public StatusController(ILogger<StatusController> logger, StatusService statusService,  IGameRepository gameRepository, ICacheContext cache)
    {
        _logger = logger;
        _statusService = statusService;
        _cache = cache;
    }

    [HttpGet("ClearStatusAndMatches")]
    public async Task ClearStatusAndMatches()
    {
        await _statusService.ClearStatusAndMatches();
        await _cache.Clear();
    }

    [HttpGet("Export")]
    public async Task<IActionResult> GetStatus()
    {
        var xls = await _statusService.ExportStatus();
        return File(xls, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
    }
}
