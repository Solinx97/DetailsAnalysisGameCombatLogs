using CombatAnalysis.Core.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace CombatAnalysis.Core.Services;

internal class CacheService : ICacheService
{
    private readonly IMemoryCache _cache;

    public CacheService()
    {
        _cache = new MemoryCache(new MemoryCacheOptions());
    }

    public void SaveDataToCache<TModel>(string key, TModel data, int expirationInMinutes = 30)
        where TModel : class
    {
        _cache.Set(key, data, new MemoryCacheEntryOptions
        {
            SlidingExpiration = TimeSpan.FromMinutes(expirationInMinutes)
        });
    }

    public TModel GetDataFromCache<TModel>(string key)
        where TModel : class
    {
        _cache.TryGetValue(key, out TModel data);

        return data;
    }

    public void RemoveDataFromCache(string key)
    {
        _cache.Remove(key);
    }
}
