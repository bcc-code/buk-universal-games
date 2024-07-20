using System.Text.Json;

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
            var databaseData = await fetchFromDatabase();
            var cachedData = await _cacheContext.Get<T>(key);

            if (cachedData == null)
            {
                if (databaseData == null)
                {
                    throw new Exception("database returned null value, we will be refetching this every time the function is run.");
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
