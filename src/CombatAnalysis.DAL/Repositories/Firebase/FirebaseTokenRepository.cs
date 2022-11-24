using CombatAnalysis.DAL.Data;
using CombatAnalysis.DAL.Entities.Authentication;
using CombatAnalysis.DAL.Interfaces;
using Firebase.Database.Query;

namespace CombatAnalysis.DAL.Repositories.Firebase;

public class FirebaseTokenRepository : ITokenRepository
{
    private readonly FirebaseContext _context;

    public FirebaseTokenRepository(FirebaseContext context)
    {
        _context = context;
    }

    public async Task<RefreshToken> CreateAsync(RefreshToken item)
    {
        var itemPropertyId = item.GetType().GetProperty(nameof(RefreshToken.Id));
        if (itemPropertyId.PropertyType == typeof(int))
        {
            var hashCodeToId = int.Parse(Guid.NewGuid().GetHashCode().ToString());
            var newId = hashCodeToId >= 0 ? hashCodeToId : -hashCodeToId;
            itemPropertyId.SetValue(item, newId);
        }

        var result = await _context.FirebaseClient
                     .Child(nameof(RefreshToken))
                     .PostAsync(item);
        return result.Object;
    }

    public async Task<int> DeleteAsync(RefreshToken item)
    {
        var data = await _context.FirebaseClient
              .Child(nameof(RefreshToken))
              .OnceAsync<RefreshToken>();

        var id = item.GetType().GetProperty(nameof(RefreshToken.Id)).GetValue(item);
        var result = data.Select(x => new KeyValuePair<string, RefreshToken>(x.Key, x.Object))
                        .AsEnumerable()
                        .FirstOrDefault(x => x.Value.GetType().GetProperty(nameof(RefreshToken.Id)).GetValue(x.Value).Equals(id));

        await _context.FirebaseClient
                     .Child(item.GetType().Name)
                     .Child(result.Key)
                     .DeleteAsync();

        var checkResult = await _context.FirebaseClient
              .Child(nameof(RefreshToken))
              .Child(result.Key)
              .OnceSingleAsync<RefreshToken>();

        return checkResult == null ? 1 : 0;
    }

    public async Task<RefreshToken> GetByTokenAsync(string token)
    {
        var data = await _context.FirebaseClient
              .Child(nameof(RefreshToken))
              .OnceAsync<RefreshToken>();

        var result = data.Select(x => x.Object)
            .AsEnumerable()
            .FirstOrDefault(x => x.GetType().GetProperty(nameof(RefreshToken.Token)).GetValue(x).Equals(token));

        return result;
    }

    public async Task<IEnumerable<RefreshToken>> GetAllByUserAsync(string userId)
    {
        var data = await _context.FirebaseClient
              .Child(nameof(RefreshToken))
              .OnceAsync<RefreshToken>();

        var result = data.Select(x => x.Object)
            .AsEnumerable()
            .Where(x => x.GetType().GetProperty(nameof(RefreshToken.UserId)).GetValue(x).Equals(userId));

        return result;
    }

    public async Task<int> UpdateAsync(RefreshToken item)
    {
        var data = await _context.FirebaseClient
              .Child(nameof(RefreshToken))
              .OnceAsync<RefreshToken>();

        var id = item.GetType().GetProperty(nameof(RefreshToken.Id)).GetValue(item);
        var result = data.Select(x => new KeyValuePair<string, RefreshToken>(x.Key, x.Object))
            .AsEnumerable()
            .FirstOrDefault(x => x.Value.GetType().GetProperty(nameof(RefreshToken.Id)).GetValue(x.Value).Equals(id));

        await _context.FirebaseClient
                     .Child(item.GetType().Name)
                     .Child(result.Key)
                     .PutAsync(item);

        var checkResult = await _context.FirebaseClient
              .Child(nameof(RefreshToken))
              .Child(result.Key)
              .OnceSingleAsync<RefreshToken>();

        return checkResult != null ? 1 : 0;
    }
}
