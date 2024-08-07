﻿using System.Text.Json;

namespace Buk.UniversalGames.Data
{
    public class ValidatingCacheService
    {
        private readonly ICacheContext _cacheContext;

        public ValidatingCacheService(ICacheContext cacheContext)
        {
            _cacheContext = cacheContext ?? throw new ArgumentNullException(nameof(cacheContext));
        }

        public async Task<T> WriteThrough<T>(string key, Func<Task<T>> fetchFromDatabase)
        {
            var validateCache = false;
            if (validateCache)
            {
                // Fetch from database and cache, compare them
                var databaseData = await fetchFromDatabase();
                var cachedData = await _cacheContext.Get<T>(key);

                if (cachedData == null)
                {
                    if (databaseData == null)
                    {
                        return databaseData;
                    }
                    await _cacheContext.Set(key, databaseData);
                    return databaseData;
                }

                if (!JsonSerializer.Serialize(cachedData).Equals(JsonSerializer.Serialize(databaseData)))
                {
                    string prettyPrintedDifferenceString = JsonSerializer.Serialize(
                        new { cache = cachedData, database = databaseData },
                        new JsonSerializerOptions() { WriteIndented = true }
                    );
                    throw new InvalidOperationException($"Cache data mismatch for key {key}:\n{prettyPrintedDifferenceString}");
                }

                return cachedData;
            }
            else
            {
                // Fetch from cache first, then from database if cache is empty
                var cachedData = await _cacheContext.Get<T>(key);
                if (cachedData != null)
                {
                    return cachedData;
                }

                var databaseData = await fetchFromDatabase();
                if (databaseData == null)
                {
                    return databaseData;
                }

                await _cacheContext.Set(key, databaseData);
                return databaseData;
            }
        }

        public Task Clear()
        {
            return _cacheContext.Clear();
        }

        public Task Remove(string key)
        {
            return _cacheContext.Remove(key);
        }
    }
}
