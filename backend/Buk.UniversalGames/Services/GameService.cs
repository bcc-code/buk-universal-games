using Buk.UniversalGames.Interfaces;
using Microsoft.Extensions.Logging;

namespace Buk.UniversalGames.Services
{
    public class GameService : IStickerService
    {
        private readonly ILogger<GameService> _logger;

        public GameService(ILogger<GameService> logger)
        {
            _logger = logger;
        }
    }
}
