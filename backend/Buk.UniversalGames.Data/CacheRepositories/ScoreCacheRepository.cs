using Buk.UniversalGames.Data.Interfaces;
using Buk.UniversalGames.Data.Repositories;
using Microsoft.Extensions.Logging;

namespace Buk.UniversalGames.Data.CacheRepositories
{
    public class ScoreCacheRepository : IScoreRepository
    {
        private readonly ILogger<ScoreCacheRepository> _logger;
        private readonly ScoreDataRepository _data;

        public ScoreCacheRepository(ILogger<ScoreCacheRepository> logger, DataContext dataContext)
        {
            _logger = logger;
            _data = new ScoreDataRepository(dataContext);
        }
    }
}
