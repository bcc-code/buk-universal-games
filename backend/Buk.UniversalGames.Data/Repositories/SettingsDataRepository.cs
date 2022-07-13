using Buk.UniversalGames.Data.Interfaces;

namespace Buk.UniversalGames.Data.Repositories
{
    public class SettingsDataRepository : ISettingsRepository
    {
        private readonly DataContext _db;

        public SettingsDataRepository(DataContext db)
        {
            _db = db;
        }

        public string? GetSettings(string key)
        {
            return _db.Settings.FirstOrDefault(s => s.Key == key)?.Value;
        }
    }
}
