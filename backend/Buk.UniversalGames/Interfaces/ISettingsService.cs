namespace Buk.UniversalGames.Interfaces
{
    public interface ISettingsService
    {
        Task<string?> GetSettings(string key, string? defaultValue = null);
    }
}
