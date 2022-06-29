namespace Buk.UniversalGames.Data
{
    public interface ICacheContext
    {
        T? Get<T>(string key);
        T Set<T>(string key, T value, int expirationHours = 24, int slidingMinutes = 60);
    }
}
