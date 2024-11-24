namespace CombatAnalysis.Core.Interfaces;

public interface ICacheService
{
    void SaveDataToCache<TModel>(string key, TModel data, int expirationInMinutes = 30) where TModel : class;

    TModel GetDataFromCache<TModel>(string key) where TModel : class;

    void RemoveDataFromCache(string key);
}