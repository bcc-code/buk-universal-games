namespace Buk.UniversalGames.Interfaces
{
    public interface ISettingsService
    {
        string? GetSettings(string key, string? defaultValue = null);
    }
}
