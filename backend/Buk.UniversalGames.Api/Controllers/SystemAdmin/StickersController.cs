using Buk.UniversalGames.Api.Authorization;
using Buk.UniversalGames.Data.Models;
using Buk.UniversalGames.Interfaces;
using Buk.UniversalGames.Library.Enums;
using Microsoft.AspNetCore.Mvc;

namespace Buk.UniversalGames.Api.Controllers.SystemAdmin;

[Obsolete("Deprecated")]
[ApiController]
[TeamType(TeamType.SystemAdmin)]
[Route("{code}/SystemAdmin/[controller]")]
public class StickersController : ControllerBase
{
    private readonly ILogger<StickersController> _logger;
    private readonly IStickerService _stickerService;

    public StickersController(ILogger<StickersController> logger, IStickerService stickerService)
    {
        _logger = logger;
        _stickerService = stickerService;
    }

    [HttpGet]
    public async Task<IActionResult> GetStickers()
    {
        var xls = await _stickerService.ExportStickers();
        return File(xls, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
    }

    [HttpGet("{leagueId}")]
    public async Task<ActionResult<List<Sticker>>> GetStickers(int leagueId)
    {
        return await _stickerService.GetStickers(leagueId);
    }

    [HttpPost("SetRandomStickerPoints")]
    public async Task SetRandomStickerPoints()
    {
        await _stickerService.SetRandomStickerPoints();
    }
}
