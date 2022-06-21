using System.Diagnostics;
using Buk.UniversalGames.Interfaces;
using Buk.UniversalGames.Library.Cultures;
using Buk.UniversalGames.Library.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Buk.UniversalGames.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class QRController : ControllerBase
{
    private readonly ILogger<QRController> _logger;
    private readonly IStickerService _stickerService;

    public QRController(ILogger<QRController> logger, IStickerService stickerService)
    {
        _logger = logger;
        _stickerService = stickerService;
    }


    [HttpGet("{stickerCode}")]
    public IActionResult GetStickerQR(string stickerCode)
    {
        byte[]? qr = null;
        
        try
        {
            qr = _stickerService.GetStickerQR(stickerCode, 20);
        }
        catch (Exception ex)
        {
            return new JsonResult(new 
            {
                Message = ex.Message,
                InnerException = ex.InnerException,
                StackTrace = ex.StackTrace
            });
        }

        if (qr == null)
            throw new BadRequestException(Strings.UnknownStickerCode);
        return File(qr, "image/png");
    }
}
