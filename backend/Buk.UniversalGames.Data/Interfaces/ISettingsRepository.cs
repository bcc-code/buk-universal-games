namespace Buk.UniversalGames.Data.Interfaces
{
    public interface ISettingsRepository
    {
        string? GetSettings(string key);
    }
}
