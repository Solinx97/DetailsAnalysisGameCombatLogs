using CombatAnalysis.CustomerDAL.Data;
using CombatAnalysis.CustomerDAL.Entities;
using CombatAnalysis.CustomerDAL.Interfaces;
using Firebase.Database.Query;

namespace CombatAnalysis.CustomerDAL.Repositories.Firebase;

public class FIrebaseUserRepository : IUserRepository
{
    private readonly FirebaseContext _context;

    public FIrebaseUserRepository(FirebaseContext context)
    {
        _context = context;
    }

    public async Task<AppUser> CreateAsync(AppUser item)
    {
        var itemPropertyId = item.GetType().GetProperty(nameof(AppUser.Id));
        if (itemPropertyId?.PropertyType == typeof(int))
        {
            var hashCodeToId = int.Parse(Guid.NewGuid().GetHashCode().ToString());
            var newId = hashCodeToId >= 0 ? hashCodeToId : -hashCodeToId;
            itemPropertyId.SetValue(item, newId);
        }

        var result = await _context.FirebaseClient
                     .Child(nameof(AppUser))
                     .PostAsync(item);
        return result.Object;
    }

    public async Task<int> DeleteAsync(AppUser item)
    {
        var data = await _context.FirebaseClient
              .Child(nameof(AppUser))
              .OnceAsync<AppUser>();

        var id = item.GetType().GetProperty(nameof(AppUser.Id))?.GetValue(item);
        var result = data.Select(x => new KeyValuePair<string, AppUser>(x.Key, x.Object))
                        .AsEnumerable()
                        .FirstOrDefault(x => x.Value.GetType().GetProperty(nameof(AppUser.Id)).GetValue(x.Value).Equals(id));

        await _context.FirebaseClient
                     .Child(item.GetType().Name)
                     .Child(result.Key)
                     .DeleteAsync();

        var checkResult = await _context.FirebaseClient
              .Child(nameof(AppUser))
              .Child(result.Key)
              .OnceSingleAsync<AppUser>();

        return checkResult == null ? 1 : 0;
    }

    public async Task<IEnumerable<AppUser>> GetAllAsync()
    {
        var data = await _context.FirebaseClient
          .Child(nameof(AppUser))
          .OnceAsync<AppUser>();

        var result = data.Select(x => x.Object).AsEnumerable();
        return result;
    }

    public async Task<AppUser> GetAsync(string email, string password)
    {
        var data = await _context.FirebaseClient
              .Child(nameof(AppUser))
              .OnceAsync<AppUser>();

        var result = data.Select(x => x.Object)
            .AsEnumerable()
            .FirstOrDefault(x => x.GetType().GetProperty(nameof(AppUser.Email)).GetValue(x).Equals(email)
                    && x.GetType().GetProperty(nameof(AppUser.Password)).GetValue(x).Equals(password));

        return result;
    }

    public async Task<AppUser> GetAsync(string email)
    {
        var data = await _context.FirebaseClient
              .Child(nameof(AppUser))
              .OnceAsync<AppUser>();

        var result = data.Select(x => x.Object)
            .AsEnumerable()
            .FirstOrDefault(x => x.GetType().GetProperty(nameof(AppUser.Email)).GetValue(x).Equals(email));

        return result;
    }

    public async Task<AppUser> GetByIdAsync(string id)
    {
        var data = await _context.FirebaseClient
              .Child(nameof(AppUser))
              .OnceAsync<AppUser>();

        var result = data.Select(x => x.Object)
            .AsEnumerable()
            .FirstOrDefault(x => x.GetType().GetProperty(nameof(AppUser.Id)).GetValue(x).Equals(id));

        return result;
    }

    public async Task<int> UpdateAsync(AppUser item)
    {
        var data = await _context.FirebaseClient
              .Child(nameof(AppUser))
              .OnceAsync<AppUser>();

        var id = item.GetType().GetProperty(nameof(AppUser.Id)).GetValue(item);
        var result = data.Select(x => new KeyValuePair<string, AppUser>(x.Key, x.Object))
            .AsEnumerable()
            .FirstOrDefault(x => x.Value.GetType().GetProperty(nameof(AppUser.Id)).GetValue(x.Value).Equals(id));

        await _context.FirebaseClient
                     .Child(item.GetType().Name)
                     .Child(result.Key)
                     .PutAsync(item);

        var checkResult = await _context.FirebaseClient
              .Child(nameof(AppUser))
              .Child(result.Key)
              .OnceSingleAsync<AppUser>();

        return checkResult != null ? 1 : 0;
    }
}
