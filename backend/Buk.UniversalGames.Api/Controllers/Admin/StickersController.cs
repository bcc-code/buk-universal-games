using Buk.UniversalGames.Api.Authorization;
using Buk.UniversalGames.Data.Models;
using Buk.UniversalGames.Interfaces;
using Buk.UniversalGames.Library.Cultures;
using Buk.UniversalGames.Library.Enums;
using Buk.UniversalGames.Library.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Buk.UniversalGames.Api.Controllers.Admin;

[ApiController]
[TeamType(TeamType.Admin,TeamType.SystemAdmin)]
[Route("{code}/Admin/[controller]")]
public class StickersController : ControllerBase
{
    private readonly ILogger<StickersController> _logger;
    private readonly IStickerService _stickerService;

    public StickersController(ILogger<StickersController> logger, IStickerService stickerService)
    {
        _logger = logger;
        _stickerService = stickerService;
    }

    [HttpGet("{leagueId}")]
    public ActionResult<List<Sticker>> GetStickers(int leagueId)
    {
        return _stickerService.GetStickers(leagueId);
    }

    [HttpGet("QR/{stickerId}/{size}")]
    public IActionResult GetStickerQRById(int stickerId, int size = 20)
    {
        var qr = _stickerService.GetStickerQRById(stickerId, size);
        if(qr == null)
            throw new BadRequestException(Strings.UnknownStickerCode);
        return File(qr, "image/jpeg");
    }
}
