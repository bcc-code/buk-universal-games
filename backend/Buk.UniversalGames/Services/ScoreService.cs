using Buk.UniversalGames.Interfaces;
using Microsoft.Extensions.Logging;

namespace Buk.UniversalGames.Services
{
    public class ScoreService : IScoreService
    {
        private readonly ILogger<ScoreService> _logger;

        public ScoreService(ILogger<ScoreService> logger)
        {
            _logger = logger;
        }
    }
}
