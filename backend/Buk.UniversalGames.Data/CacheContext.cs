using System.Text;
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

        public T Set<T>(string key, T value)
        {
            var timeOut = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(250),
                SlidingExpiration = TimeSpan.FromHours(250)
            };

            _cache.SetString(key, JsonSerializer.Serialize(value), timeOut);

            var cacheKeys = GetCacheKeys();
            cacheKeys.Add(key);
            SetCacheKeys(cacheKeys);

            return value;
        }

        public void Remove(string key)
        {
            _cache.Remove(key);

            var cacheKeys = GetCacheKeys();
            cacheKeys.Remove(key);
            SetCacheKeys(cacheKeys);
        }

        public void Clear()
        {
            var cacheKeys = GetCacheKeys();
            foreach (var cacheKey in cacheKeys)
                _cache.Remove(cacheKey);
            _cache.Remove("CacheKeys");
        }


        private List<string> GetCacheKeys()
        {
            var keys = _cache.Get("CacheKeys");
            if (keys == null)
                return new List<string>();
            return Encoding.Default.GetString(keys).Split('|').ToList();
        }

        private void SetCacheKeys(List<string> keys)
        {
            var timeOut = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(250),
                SlidingExpiration = TimeSpan.FromHours(250),
            };

            var keysString = String.Join('|', keys);
            _cache.Set("CacheKeys", Encoding.ASCII.GetBytes(keysString), timeOut);
        }
    }
}
