using Buk.UniversalGames.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Buk.UniversalGames.Data.Repositories
{
    public class SettingsDataRepository : ISettingsRepository
    {
        private readonly DataContext _db;

        public SettingsDataRepository(DataContext db)
        {
            _db = db;
        }

        public async Task<string?> GetSettings(string key)
        {
            return (await _db.Settings.FirstOrDefaultAsync(s => s.Key == key))?.Value;
        }
    }
}
