using CombatAnalysis.DAL.Data;
using CombatAnalysis.DAL.Entities.Authentication;
using CombatAnalysis.DAL.Interfaces;
using Firebase.Database.Query;

namespace CombatAnalysis.DAL.Repositories.Firebase;

public class FirebaseSecretRepository : IAppSecret
{
    private readonly FirebaseContext _context;

    public FirebaseSecretRepository(FirebaseContext context)
    {
        _context = context;
    }

    public async Task<Secret> CreateAsync(Secret item)
    {
        var itemPropertyId = item.GetType().GetProperty(nameof(Secret.Id));
        if (itemPropertyId?.PropertyType == typeof(int))
        {
            var hashCodeToId = int.Parse(Guid.NewGuid().GetHashCode().ToString());
            var newId = hashCodeToId >= 0 ? hashCodeToId : -hashCodeToId;
            itemPropertyId.SetValue(item, newId);
        }

        var result = await _context.FirebaseClient
                     .Child(nameof(Secret))
                     .PostAsync(item);
        return result.Object;
    }

    public async Task<int> DeleteAsync(Secret item)
    {
        var data = await _context.FirebaseClient
              .Child(nameof(Secret))
              .OnceAsync<Secret>();

        var id = item.GetType().GetProperty(nameof(Secret.Id))?.GetValue(item);
        var result = data.Select(x => new KeyValuePair<string, Secret>(x.Key, x.Object))
                        .AsEnumerable()
                        .FirstOrDefault(x => x.Value.GetType().GetProperty(nameof(Secret.Id)).GetValue(x.Value).Equals(id));

        await _context.FirebaseClient
                     .Child(item.GetType().Name)
                     .Child(result.Key)
                     .DeleteAsync();

        var checkResult = await _context.FirebaseClient
              .Child(nameof(Secret))
              .Child(result.Key)
              .OnceSingleAsync<Secret>();

        return checkResult == null ? 1 : 0;
    }

    public async Task<Secret> GetByIdAsync(int id)
    {
        var data = await _context.FirebaseClient
              .Child(nameof(Secret))
              .OnceAsync<Secret>();

        var result = data.Select(x => x.Object)
            .AsEnumerable()
            .FirstOrDefault(x => x.GetType().GetProperty(nameof(Secret.Id)).GetValue(x).Equals(id));

        return result;
    }

    public async Task<IEnumerable<Secret>> GetAllAsync()
    {
        var data = await _context.FirebaseClient
              .Child(nameof(Secret))
              .OnceAsync<Secret>();

        var result = data.Select(x => x.Object)
            .AsEnumerable();

        return result;
    }

    public async Task<int> UpdateAsync(Secret item)
    {
        var data = await _context.FirebaseClient
              .Child(typeof(Secret).Name)
              .OnceAsync<Secret>();

        var id = item.GetType().GetProperty("Id")?.GetValue(item);
        var result = data.Select(x => new KeyValuePair<string, Secret>(x.Key, x.Object))
            .AsEnumerable()
            .FirstOrDefault(x => x.Value.GetType().GetProperty("Id").GetValue(x.Value).Equals(id));

        await _context.FirebaseClient
                     .Child(item.GetType().Name)
                     .Child(result.Key)
                     .PutAsync(item);

        var checkResult = await _context.FirebaseClient
              .Child(typeof(Secret).Name)
              .Child(result.Key)
              .OnceSingleAsync<Secret>();

        return checkResult != null ? 1 : 0;
    }
}
