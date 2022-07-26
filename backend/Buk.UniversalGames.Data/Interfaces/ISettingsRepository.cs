namespace Buk.UniversalGames.Data.Interfaces
{
    public interface ISettingsRepository
    {
        Task<string?> GetSettings(string key);
    }
}
