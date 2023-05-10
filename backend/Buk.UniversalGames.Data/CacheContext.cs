using System.Reflection.Metadata;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using StackExchange.Redis;

namespace Buk.UniversalGames.Data
{
    public class CacheContext : ICacheContext
    {
        private readonly IDistributedCache _cache;
        private static readonly Encoding _keysEncoding = Encoding.UTF8;

        public CacheContext(IDistributedCache cache)
        {
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
        }

        public Task<T?> Get<T>(string key)
        {
            return Retry(async () =>
            {
                var value = await _cache.GetStringAsync(key);
                return value != null ? JsonSerializer.Deserialize<T>(value) : default;
            });

        }

        public Task<T> Set<T>(string key, T value)
        {
            return Retry(async () =>
            {
                var expirationOptions = new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(250),
                    SlidingExpiration = TimeSpan.FromHours(250)
                };
                
                await _cache.SetStringAsync(key, JsonSerializer.Serialize(value), expirationOptions);

                var cacheKeys = await GetCacheKeys();
                cacheKeys.Add(key);
                await SetCacheKeys(cacheKeys);

                return value;
            });
            
        }

        public Task Remove(string key)
        {
            return Retry(async () =>
            {
                await _cache.RemoveAsync(key);

                var cacheKeys = await GetCacheKeys();
                cacheKeys.Remove(key);
                await SetCacheKeys(cacheKeys);
                return true;
            });
            
        }

        public Task Clear()
        {
            return Retry(async () =>
            {
                var cacheKeys = await GetCacheKeys();
                foreach (var cacheKey in cacheKeys)
                    await _cache.RemoveAsync(cacheKey);
                await _cache.RemoveAsync("CacheKeys");
                return true;
            });
            
        }


        Task<List<string>> GetCacheKeys()
        {
            return Retry(async () =>
            {
                var keys = await _cache.GetAsync("CacheKeys");
                if (keys == null)
                    return new List<string>();
                return Encoding.ASCII.GetString(keys).Split('|').ToList();
            });
        }

        Task SetCacheKeys(List<string> keys)
        {
            return Retry(async () =>
            {
                var timeOut = new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(250),
                    SlidingExpiration = TimeSpan.FromHours(250),
                };

                var keysString = String.Join('|', keys);
                await _cache.SetAsync("CacheKeys", _keysEncoding.GetBytes(keysString), timeOut);
                return true;
            });
        }

        static async Task<T> Retry<T>(Func<Task<T>> action)
        {
            int attempts = 0;
            while (true)
            {
                try
                {
                    return await action();
                }
                catch (Exception)
                {
                    if (attempts > 5) { throw; }
                    attempts++;
                    await Task.Delay(TimeSpan.FromMilliseconds(attempts * attempts * 100));
                }
            }
        }
    }
}
