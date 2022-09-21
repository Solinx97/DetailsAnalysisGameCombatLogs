using CombatAnalysis.DAL.Data.SQL;
using CombatAnalysis.DAL.Entities.Authentication;
using CombatAnalysis.DAL.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CombatAnalysis.DAL.Repositories.SQL.StoredProcedure
{
    public class SQLSPTokenRepository : ITokenRepository
    {
        private readonly SQLContext _context;

        public SQLSPTokenRepository(SQLContext context)
        {
            _context = context;
        }

        async Task<RefreshToken> ITokenRepository.CreateAsync(RefreshToken item)
        {
            var properties = item.GetType().GetProperties();
            var procedureParams = new List<SqlParameter>();
            var procedureParamNames = new StringBuilder();

            for (int i = 0; i < properties.Length; i++)
            {
                if (properties[i].CanWrite)
                {
                    procedureParams.Add(new SqlParameter(properties[i].Name, properties[i].GetValue(item)));
                    procedureParamNames.Append($"@{properties[i].Name},");
                }
            }
            procedureParamNames.Remove(procedureParamNames.Length - 1, 1);

            var res = await Task.Run(() => _context.Set<RefreshToken>().FromSqlRaw($"InsertInto{item.GetType().Name} {procedureParamNames}", procedureParams.ToArray())
                                                .AsEnumerable().FirstOrDefault());

            return res;
        }

        async Task<int> ITokenRepository.DeleteAsync(RefreshToken item)
        {
            var property = item.GetType().GetProperty("Id");
            var data = await _context.Database
                                .ExecuteSqlRawAsync($"Delete{item.GetType().Name}ById {property.Name}", property.GetValue(item));

            return data;
        }

        async Task<RefreshToken> ITokenRepository.GetByTokenAsync(string token)
        {
            var result = new RefreshToken();
            var data = await _context.Set<RefreshToken>()
                                .FromSqlRaw($"GetAll{nameof(RefreshToken)}")
                                .ToListAsync();

            foreach (var item in data)
            {
                if (item.GetType().GetProperty(nameof(RefreshToken.Token)).Equals(token))
                {
                    result = item;
                    break;
                }
            }

            return result;
        }

        async Task<IEnumerable<RefreshToken>> ITokenRepository.GetAllByUserAsync(string userId)
        {
            var result = new List<RefreshToken>();
            var data = await _context.Set<RefreshToken>()
                                .FromSqlRaw($"GetAll{nameof(RefreshToken)}")
                                .ToListAsync();

            foreach (var item in data)
            {
                if (item.GetType().GetProperty(nameof(RefreshToken.UserId)).Equals(userId))
                {
                    result.Add(item);
                }
            }

            return result;
        }

        async Task<int> ITokenRepository.UpdateAsync(RefreshToken item)
        {
            var properties = item.GetType().GetProperties();
            var procedureParams = new List<SqlParameter>();
            var procedureParamNames = new StringBuilder();
            for (int i = 0; i < properties.Length; i++)
            {
                if (properties[i].CanWrite)
                {
                    procedureParams.Add(new SqlParameter(properties[i].Name, properties[i].GetValue(item)));
                    procedureParamNames.Append($"@{properties[i].Name},");
                }
            }
            procedureParamNames.Remove(procedureParamNames.Length - 1, 1);

            var data = await _context.Database
                                .ExecuteSqlRawAsync($"Update{item.GetType().Name} {procedureParamNames}", procedureParams);

            return data;
        }
    }
}
