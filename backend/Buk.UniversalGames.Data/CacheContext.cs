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

        public async Task<T?> Get<T>(string key)
        {
            var value = await _cache.GetStringAsync(key);
            return value != null ? JsonSerializer.Deserialize<T>(value) : default;
        }

        public async Task<T> Set<T>(string key, T value)
        {
            var timeOut = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(250),
                SlidingExpiration = TimeSpan.FromHours(250)
            };

            await _cache.SetStringAsync(key, JsonSerializer.Serialize(value), timeOut);

            var cacheKeys = await GetCacheKeys();
            cacheKeys.Add(key);
            await SetCacheKeys(cacheKeys);

            return value;
        }

        public async Task Remove(string key)
        {
            await _cache.RemoveAsync(key);

            var cacheKeys = await GetCacheKeys();
            cacheKeys.Remove(key);
            await SetCacheKeys(cacheKeys);
        }

        public async Task Clear()
        {
            var cacheKeys = await GetCacheKeys();
            foreach (var cacheKey in cacheKeys)
                await _cache.RemoveAsync(cacheKey);
            await _cache.RemoveAsync("CacheKeys");
        }


        private async Task<List<string>> GetCacheKeys()
        {
            var keys = await _cache.GetAsync("CacheKeys");
            if (keys == null)
                return new List<string>();
            return Encoding.Default.GetString(keys).Split('|').ToList();
        }

        private async Task SetCacheKeys(List<string> keys)
        {
            var timeOut = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(250),
                SlidingExpiration = TimeSpan.FromHours(250),
            };

            var keysString = String.Join('|', keys);
            await _cache.SetAsync("CacheKeys", Encoding.ASCII.GetBytes(keysString), timeOut);
        }
    }
}
