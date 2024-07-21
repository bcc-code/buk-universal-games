namespace Buk.UniversalGames.Data
{
    public interface ICacheContext
    {
        Task<T?> Get<T>(string key);
        Task<T> Set<T>(string key, T value, double? cacheMinutes = 5);
        Task Remove(string key);
        Task Clear();
    }
}
