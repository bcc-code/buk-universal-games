namespace Buk.UniversalGames.Interfaces
{
    public interface ISettingsService
    {
        Task<string?> GetSetting(string key, string? defaultValue = null);
    }
}
