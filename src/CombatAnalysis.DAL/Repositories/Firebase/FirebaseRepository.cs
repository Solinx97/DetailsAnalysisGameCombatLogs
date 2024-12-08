using CombatAnalysis.DAL.Data;
using CombatAnalysis.DAL.Interfaces;
using CombatAnalysis.DAL.Interfaces.Entities;
using Firebase.Database.Query;

namespace CombatAnalysis.DAL.Repositories.Firebase;

internal class FirebaseRepository<TModel> : IGenericRepository<TModel>
    where TModel : class, IEntity
{
    private readonly FirebaseContext _context;

    public FirebaseRepository(FirebaseContext context)
    {
        _context = context;
    }

    public async Task<TModel> CreateAsync(TModel item)
    {
        var result = await _context.FirebaseClient
                     .Child(item.GetType().Name)
                     .PostAsync(item);
        return result.Object;
    }

    public async Task<int> DeleteAsync(int id)
    {
        var data = await _context.FirebaseClient
              .Child(typeof(TModel).Name)
              .OnceAsync<TModel>();

        var result = data.Select(x => new KeyValuePair<string, TModel>(x.Key, x.Object))
            .AsEnumerable()
            .FirstOrDefault(x => x.Value.Id == id);

        await _context.FirebaseClient
                     .Child(typeof(TModel).Name)
                     .Child(result.Key)
                     .DeleteAsync();

        var checkResult = await _context.FirebaseClient
              .Child(typeof(TModel).Name)
              .Child(result.Key)
              .OnceSingleAsync<TModel>();

        return checkResult == null ? 1 : 0;
    }

    public async Task<IEnumerable<TModel>> GetAllAsync()
    {
        var data = await _context.FirebaseClient
          .Child(typeof(TModel).Name)
          .OnceAsync<TModel>();

        var result = data.Select(x => x.Object).AsEnumerable();
        return result;
    }

    public async Task<TModel> GetByIdAsync(int id)
    {
        var data = await _context.FirebaseClient
              .Child(typeof(TModel).Name)
              .OnceAsync<TModel>();

        var result = data.Select(x => x.Object)
            .AsEnumerable()
            .FirstOrDefault(x => x.Id == id);

        return result;
    }

    public IEnumerable<TModel> GetByParam(string paramName, object value)
    {
        var data = _context.FirebaseClient
              .Child(typeof(TModel).Name)
              .OnceAsync<TModel>()
              .GetAwaiter()
              .GetResult();

        var result = data.Select(x => x.Object)
            .AsEnumerable()
            .Where(x => x.GetType().GetProperty(paramName).GetValue(x).Equals(value));

        return result;
    }

    public async Task<int> UpdateAsync(TModel item)
    {
        var data = await _context.FirebaseClient
              .Child(typeof(TModel).Name)
              .OnceAsync<TModel>();

        var result = data.Select(x => new KeyValuePair<string, TModel>(x.Key, x.Object))
            .AsEnumerable()
            .FirstOrDefault(x => x.Value.Id == item.Id);

        await _context.FirebaseClient
                     .Child(item.GetType().Name)
                     .Child(result.Key)
                     .PutAsync(item);

        var checkResult = await _context.FirebaseClient
              .Child(typeof(TModel).Name)
              .Child(result.Key)
              .OnceSingleAsync<TModel>();

        return checkResult != null ? 1 : 0;
    }
}
