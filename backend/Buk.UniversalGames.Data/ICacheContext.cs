namespace Buk.UniversalGames.Data
{
    public interface ICacheContext
    {
        Task<T?> Get<T>(string key);
        Task<T> Set<T>(string key, T value);
        Task Remove(string key);
        Task Clear();
    }
}
