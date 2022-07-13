using Buk.UniversalGames.Data.Interfaces;
using Buk.UniversalGames.Interfaces;
using Microsoft.Extensions.Logging;

namespace Buk.UniversalGames.Services
{
    public class SettingsService : ISettingsService
    {
        private readonly ILogger<SettingsService> _logger;
        private readonly ISettingsRepository _settingsRepository;

        public SettingsService(ILogger<SettingsService> logger, ISettingsRepository settingsRepository)
        {
            _logger = logger;
            _settingsRepository = settingsRepository;
        }

        public string? GetSettings(string key, string? defaultValue = null)
        {
            var settings = _settingsRepository.GetSettings(key);
            return settings ?? defaultValue;
        }
    }
}
