using Buk.UniversalGames.Api.Authorization;
using Buk.UniversalGames.Data.Models;
using Buk.UniversalGames.Interfaces;
using Buk.UniversalGames.Library.Enums;
using Microsoft.AspNetCore.Mvc;

namespace Buk.UniversalGames.Api.Controllers.SystemAdmin;

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
    public IActionResult GetStickers()
    {
        var xls = _stickerService.ExportStickers();
        return File(xls, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
    }

    [HttpGet("{leagueId}")]
    public ActionResult<List<Sticker>> GetStickers(int leagueId)
    {
        return _stickerService.GetStickers(leagueId);
    }

    [HttpPost("SetRandomStickerPoints")]
    public void SetRandomStickerPoints()
    {
        _stickerService.SetRandomStickerPoints();
    }
}
