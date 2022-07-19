using Buk.UniversalGames.Api.Authorization;
using Buk.UniversalGames.Data;
using Buk.UniversalGames.Library.Enums;
using Microsoft.AspNetCore.Mvc;

namespace Buk.UniversalGames.Api.Controllers.SystemAdmin;

[ApiController]
[TeamType(TeamType.SystemAdmin)]
[Route("{code}/SystemAdmin/[controller]")]
public class CacheController : ControllerBase
{
    private readonly ILogger<CacheController> _logger;
    private readonly ICacheContext _cache;

    public CacheController(ILogger<CacheController> logger, ICacheContext cache)
    {
        _logger = logger;
        _cache = cache;
    }

    [HttpGet]
    public IActionResult ClearCache()
    {
        _cache.Clear();
        return Ok();
    }
}
