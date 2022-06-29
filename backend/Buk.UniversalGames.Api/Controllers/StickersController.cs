using Buk.UniversalGames.Api.Authorization;
using Buk.UniversalGames.Data.Models;
using Buk.UniversalGames.Interfaces;
using Buk.UniversalGames.Models;
using Microsoft.AspNetCore.Mvc;

namespace Buk.UniversalGames.Api.Controllers;

[ApiController]
[Route("{code}/[controller]")]
public class StickersController : ControllerBase
{
    private readonly ILogger<StickersController> _logger;
    private readonly IStickerService _stickerService;

    public StickersController(ILogger<StickersController> logger, IStickerService stickerService)
    {
        _logger = logger;
        _stickerService = stickerService;
    }

    [Participant]
    [HttpPost("{stickerCode}/Scan")]
    public ActionResult<ScanResult> Scan(string stickerCode)
    {
        var team = HttpContext.Items["ValidatedTeam"] as Team;
        return _stickerService.ScanSticker(team, stickerCode);
    }
}
