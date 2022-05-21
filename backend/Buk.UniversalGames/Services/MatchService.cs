using Buk.UniversalGames.Interfaces;
using Microsoft.Extensions.Logging;

namespace Buk.UniversalGames.Services
{
    public class MatchService : IMatchService
    {
        private readonly ILogger<MatchService> _logger;

        public MatchService(ILogger<MatchService> logger)
        {
            _logger = logger;
        }
    }
}
