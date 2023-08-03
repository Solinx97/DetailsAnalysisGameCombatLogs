using CombatAnalysis.DAL.Data;
using CombatAnalysis.DAL.Entities.Authentication;
using CombatAnalysis.DAL.Interfaces;
using Firebase.Database.Query;

namespace CombatAnalysis.DAL.Repositories.Firebase;

public class FirebaseAccessTokenRepository : ITokenRepository<AccessToken>
{
    private readonly FirebaseContext _context;

    public FirebaseAccessTokenRepository(FirebaseContext context)
    {
        _context = context;
    }

    public async Task<AccessToken> CreateAsync(AccessToken item)
    {
        var itemPropertyId = item.GetType().GetProperty(nameof(AccessToken.Id));
        if (itemPropertyId?.PropertyType == typeof(int))
        {
            var hashCodeToId = int.Parse(Guid.NewGuid().GetHashCode().ToString());
            var newId = hashCodeToId >= 0 ? hashCodeToId : -hashCodeToId;
            itemPropertyId.SetValue(item, newId);
        }

        var result = await _context.FirebaseClient
                     .Child(nameof(AccessToken))
                     .PostAsync(item);
        return result.Object;
    }

    public async Task<int> DeleteAsync(AccessToken item)
    {
        var data = await _context.FirebaseClient
              .Child(nameof(AccessToken))
              .OnceAsync<AccessToken>();

        var id = item.GetType().GetProperty(nameof(AccessToken.Id))?.GetValue(item);
        var result = data.Select(x => new KeyValuePair<string, AccessToken>(x.Key, x.Object))
                        .AsEnumerable()
                        .FirstOrDefault(x => x.Value.GetType().GetProperty(nameof(AccessToken.Id)).GetValue(x.Value).Equals(id));

        await _context.FirebaseClient
                     .Child(item.GetType().Name)
                     .Child(result.Key)
                     .DeleteAsync();

        var checkResult = await _context.FirebaseClient
              .Child(nameof(AccessToken))
              .Child(result.Key)
              .OnceSingleAsync<AccessToken>();

        return checkResult == null ? 1 : 0;
    }

    public async Task<AccessToken> GetByTokenAsync(string token)
    {
        var data = await _context.FirebaseClient
              .Child(nameof(AccessToken))
              .OnceAsync<AccessToken>();

        var result = data.Select(x => x.Object)
            .AsEnumerable()
            .FirstOrDefault(x => x.GetType().GetProperty(nameof(AccessToken.Token)).GetValue(x).Equals(token));

        return result;
    }

    public async Task<IEnumerable<AccessToken>> GetAllByUserAsync(string userId)
    {
        var data = await _context.FirebaseClient
              .Child(nameof(AccessToken))
              .OnceAsync<AccessToken>();

        var result = data.Select(x => x.Object)
            .AsEnumerable()
            .Where(x => x.GetType().GetProperty(nameof(AccessToken.UserId)).GetValue(x).Equals(userId));

        return result;
    }

    public async Task<int> UpdateAsync(AccessToken item)
    {
        var data = await _context.FirebaseClient
              .Child(nameof(AccessToken))
              .OnceAsync<AccessToken>();

        var id = item.GetType().GetProperty(nameof(AccessToken.Id)).GetValue(item);
        var result = data.Select(x => new KeyValuePair<string, AccessToken>(x.Key, x.Object))
            .AsEnumerable()
            .FirstOrDefault(x => x.Value.GetType().GetProperty(nameof(AccessToken.Id)).GetValue(x.Value).Equals(id));

        await _context.FirebaseClient
                     .Child(item.GetType().Name)
                     .Child(result.Key)
                     .PutAsync(item);

        var checkResult = await _context.FirebaseClient
              .Child(nameof(AccessToken))
              .Child(result.Key)
              .OnceSingleAsync<AccessToken>();

        return checkResult != null ? 1 : 0;
    }
}
