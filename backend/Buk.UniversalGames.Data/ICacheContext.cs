namespace Buk.UniversalGames.Data
{
    public interface ICacheContext
    {
        T? Get<T>(string key);
        T Set<T>(string key, T value);
        void Remove(string key);
        void Clear();
    }
}
