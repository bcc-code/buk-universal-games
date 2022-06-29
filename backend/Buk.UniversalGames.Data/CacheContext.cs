using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;

namespace Buk.UniversalGames.Data
{
    public class CacheContext : ICacheContext
    {
        private readonly IDistributedCache _cache;

        public CacheContext(IDistributedCache cache)
        {
            _cache = cache;
        }

        public T? Get<T>(string key)
        {
            var value = _cache.GetString(key);
            return value != null ? JsonSerializer.Deserialize<T>(value) : default;
        }

        public T Set<T>(string key, T value, int expirationHours = 24, int slidingMinutes = 60)
        {
            var timeOut = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(expirationHours),
                SlidingExpiration = TimeSpan.FromMinutes(slidingMinutes)
            };

            _cache.SetString(key, JsonSerializer.Serialize(value), timeOut);

            return value;
        }
    }
}
