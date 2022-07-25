using Buk.UniversalGames.Data.Interfaces;
using Buk.UniversalGames.Data.Repositories;
using Microsoft.Extensions.Logging;

namespace Buk.UniversalGames.Data.CacheRepositories
{
    public class SettingsCacheRepository : ISettingsRepository
    {
        private readonly ILogger<SettingsCacheRepository> _logger;
        private readonly SettingsDataRepository _data;
        private readonly ICacheContext _cache;

        public SettingsCacheRepository(ILogger<SettingsCacheRepository> logger, DataContext dataContext, ICacheContext cache)
        {
            _logger = logger;
            _data = new SettingsDataRepository(dataContext);
            _cache = cache;
        }

        public async Task<string?> GetSettings(string key)
        {
            return await _data.GetSettings(key);
        }
    }
}
