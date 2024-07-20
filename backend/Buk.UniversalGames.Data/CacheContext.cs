using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;

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
            var expirationOptions = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5),
                SlidingExpiration = TimeSpan.FromHours(5)
            };
            string setValue = JsonSerializer.Serialize(value);

            return Retry(async () =>
            {
                await _cache.SetStringAsync(key, setValue, expirationOptions);

                var cacheKeys = await GetCacheKeys();
                if (!cacheKeys.Contains(key))
                {
                    cacheKeys.Add(key);
                    await SetCacheKeys(cacheKeys);
                    
                }

                LogSetOperation(key, setValue, cacheKeys);

                return value;
            });
        }

        private static void LogSetOperation<T>(string key, T value, List<string> cacheKeys)
        {
            Console.WriteLine("SETTING");
            Console.WriteLine($"    KEY: {key}");
            Console.WriteLine($"    VALUE: {JsonSerializer.Serialize(value)}");
            Console.WriteLine("NEW CACHEKEYS:");
            foreach (var cacheKey in cacheKeys)
            {
                Console.WriteLine($"    {cacheKey}");
            }
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
                {
                    if (cacheKey != StatusController.FrozenCacheKey)
                    {
                        await _cache.RemoveAsync(cacheKey);
                    }
                }

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
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5),
                    SlidingExpiration = TimeSpan.FromMinutes(5),
                };

                var keysString = String.Join('|', keys.Distinct());
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
