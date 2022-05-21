using Buk.UniversalGames.Models;
using Microsoft.AspNetCore.Mvc;

namespace Buk.UniversalGames.Api.Controllers;

[ApiController]
[Route("{code}/[controller]")]
public class StickersController : ControllerBase
{
    private readonly ILogger<StickersController> _logger;

    public StickersController(ILogger<StickersController> logger)
    {
        _logger = logger;
    }

    [HttpGet("ScanSticker/{stickerCode}")]
    public ActionResult<ScanResult> ScanSticker(string stickerCode)
    {
        return null;
    }
}
