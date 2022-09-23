using CombatAnalysis.DAL.Data;
using CombatAnalysis.DAL.Entities.User;
using CombatAnalysis.DAL.Interfaces;
using Firebase.Database.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CombatAnalysis.DAL.Repositories.Firebase
{
    public class FIrebaseUserRepository : IUserRepository
    {
        private readonly FirebaseContext _context;

        public FIrebaseUserRepository(FirebaseContext context)
        {
            _context = context;
        }

        async Task<AppUser> IUserRepository.CreateAsync(AppUser item)
        {
            var itemPropertyId = item.GetType().GetProperty(nameof(AppUser.Id));
            if (itemPropertyId.PropertyType == typeof(int))
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

        async Task<int> IUserRepository.DeleteAsync(AppUser item)
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
                         .DeleteAsync();

            var checkResult = await _context.FirebaseClient
                  .Child(nameof(AppUser))
                  .Child(result.Key)
                  .OnceSingleAsync<AppUser>();

            return checkResult == null ? 1 : 0;
        }

        async Task<IEnumerable<AppUser>> IUserRepository.GetAllAsync()
        {
            var data = await _context.FirebaseClient
              .Child(nameof(AppUser))
              .OnceAsync<AppUser>();

            var result = data.Select(x => x.Object).AsEnumerable();
            return result;
        }

        async Task<AppUser> IUserRepository.GetAsync(string email, string password)
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

        async Task<AppUser> IUserRepository.GetAsync(string email)
        {
            var data = await _context.FirebaseClient
                  .Child(nameof(AppUser))
                  .OnceAsync<AppUser>();

            var result = data.Select(x => x.Object)
                .AsEnumerable()
                .FirstOrDefault(x => x.GetType().GetProperty(nameof(AppUser.Email)).GetValue(x).Equals(email));

            return result;
        }

        async Task<AppUser> IUserRepository.GetByIdAsync(string id)
        {
            var data = await _context.FirebaseClient
                  .Child(nameof(AppUser))
                  .OnceAsync<AppUser>();

            var result = data.Select(x => x.Object)
                .AsEnumerable()
                .FirstOrDefault(x => x.GetType().GetProperty(nameof(AppUser.Id)).GetValue(x).Equals(id));

            return result;
        }

        async Task<int> IUserRepository.UpdateAsync(AppUser item)
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
}
