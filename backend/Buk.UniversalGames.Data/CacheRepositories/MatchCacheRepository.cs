using Buk.UniversalGames.Data.Interfaces;
using Buk.UniversalGames.Data.Repositories;
using Microsoft.Extensions.Logging;

namespace Buk.UniversalGames.Data.CacheRepositories
{
    public class MatchCacheRepository : IMatchRepository
    {
        private readonly ILogger<MatchCacheRepository> _logger;
        private readonly MatchDataRepository _data;

        public MatchCacheRepository(ILogger<MatchCacheRepository> logger, DataContext dataContext)
        {
            _logger = logger;
            _data = new MatchDataRepository(dataContext);
        }
    }
}
