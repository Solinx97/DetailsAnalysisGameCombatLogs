using CombatAnalysis.DAL.Data;
using CombatAnalysis.DAL.Interfaces;
using Firebase.Database.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CombatAnalysis.DAL.Repositories.Firebase
{
    public class FirebaseRepositroy<TModel, TIdType> : IGenericRepository<TModel, TIdType>
        where TModel : class
        where TIdType : notnull
    {
        private readonly FirebaseContext _context;

        public FirebaseRepositroy(FirebaseContext context)
        {
            _context = context;
        }

        async Task<TModel> IGenericRepository<TModel, TIdType>.CreateAsync(TModel item)
        {
            var itemPropertyId= item.GetType().GetProperty("Id");
            if (itemPropertyId.PropertyType == typeof(int))
            {
                var hashCodeToId = int.Parse(Guid.NewGuid().GetHashCode().ToString());
                var newId = hashCodeToId >= 0 ? hashCodeToId : -hashCodeToId;
                itemPropertyId.SetValue(item, newId);
            }
 
            var result = await _context.FirebaseClient
                         .Child(item.GetType().Name)
                         .PostAsync(item);
            return result.Object;
        }

        async Task<int> IGenericRepository<TModel, TIdType>.DeleteAsync(TModel item)
        {
            var data = await _context.FirebaseClient
                  .Child(typeof(TModel).Name)
                  .OnceAsync<TModel>();

            var id = item.GetType().GetProperty("Id").GetValue(item);
            var result = data.Select(x => new KeyValuePair<string, TModel>(x.Key, x.Object))
                .AsEnumerable()
                .FirstOrDefault(x => x.Value.GetType().GetProperty("Id").GetValue(x.Value).Equals(id));

            await _context.FirebaseClient
                         .Child(item.GetType().Name)
                         .Child(result.Key)
                         .DeleteAsync();

            var checkResult = await _context.FirebaseClient
                  .Child(typeof(TModel).Name)
                  .Child(result.Key)
                  .OnceSingleAsync<TModel>();

            return checkResult == null ? 1 : 0;
        }

        async Task<IEnumerable<TModel>> IGenericRepository<TModel, TIdType>.GetAllAsync()
        {
            var data = await _context.FirebaseClient
              .Child(typeof(TModel).Name)
              .OnceAsync<TModel>();

            var result = data.Select(x => x.Object).AsEnumerable();
            return result;
        }

        async Task<TModel> IGenericRepository<TModel, TIdType>.GetByIdAsync(TIdType id)
        {
            var data = await _context.FirebaseClient
                  .Child(typeof(TModel).Name)
                  .OnceAsync<TModel>();

            var result = data.Select(x => x.Object)
                .AsEnumerable()
                .FirstOrDefault(x => x.GetType().GetProperty("Id").GetValue(x).Equals(id));

            return result;
        }

        IEnumerable<TModel> IGenericRepository<TModel, TIdType>.GetByParam(string paramName, object value)
        {
            var data =  _context.FirebaseClient
                  .Child(typeof(TModel).Name)
                  .OnceAsync<TModel>()
                  .GetAwaiter()
                  .GetResult();

            var result = data.Select(x => x.Object)
                .AsEnumerable()
                .Where(x => x.GetType().GetProperty(paramName).GetValue(x).Equals(value));

            return result;
        }

        async Task<int> IGenericRepository<TModel, TIdType>.UpdateAsync(TModel item)
        {
            var data = await _context.FirebaseClient
                  .Child(typeof(TModel).Name)
                  .OnceAsync<TModel>();

            var id = item.GetType().GetProperty("Id").GetValue(item);
            var result = data.Select(x => new KeyValuePair<string, TModel>(x.Key, x.Object))
                .AsEnumerable()
                .FirstOrDefault(x => x.Value.GetType().GetProperty("Id").GetValue(x.Value).Equals(id));

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
}
