using Buk.UniversalGames.Api.Authorization;
using Buk.UniversalGames.Interfaces;
using Buk.UniversalGames.Library.Enums;
using Microsoft.AspNetCore.Mvc;

namespace Buk.UniversalGames.Api.Controllers.SystemAdmin;

[ApiController]
[TeamType(TeamType.SystemAdmin)]
[Route("{code}/SystemAdmin/[controller]")]
public class StatusController : ControllerBase
{
    private readonly ILogger<StatusController> _logger;
    private readonly IStatusService _statusService;

    public StatusController(ILogger<StatusController> logger, IStatusService statusService)
    {
        _logger = logger;
        _statusService = statusService;
    }

    [HttpGet("ClearStatusAndMatches")]
    public void ClearStatusAndMatches()
    {
        _statusService.ClearStatusAndMatches();
    }
}
