using Buk.UniversalGames.Interfaces;
using Microsoft.Extensions.Logging;

namespace Buk.UniversalGames.Services
{
    public class StickerService : IMatchService
    {
        private readonly ILogger<StickerService> _logger;

        public StickerService(ILogger<StickerService> logger)
        {
            _logger = logger;
        }
    }
}
