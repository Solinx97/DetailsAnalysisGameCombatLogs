﻿using CombatAnalysis.DAL.Data.SQL;
using CombatAnalysis.DAL.Entities.User;
using CombatAnalysis.DAL.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CombatAnalysis.DAL.Repositories.SQL.StoredProcedure
{
    public class SQLSPUserRepository : IUserRepository
    {
        private readonly SQLContext _context;

        public SQLSPUserRepository(SQLContext context)
        {
            _context = context;
        }

        async Task<AppUser> IUserRepository.CreateAsync(AppUser item)
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

            var data = await Task.Run(() => _context.Set<AppUser>().FromSqlRaw($"InsertInto{item.GetType().Name} {procedureParamNames}", procedureParams.ToArray())
                                                .AsEnumerable()
                                                .FirstOrDefault());

            return data;
        }

        async Task<int> IUserRepository.DeleteAsync(AppUser item)
        {
            var property = item.GetType().GetProperty("Id");
            var rowsAffected = await _context.Database
                                .ExecuteSqlRawAsync($"Delete{item.GetType().Name}ById {property.Name}", property.GetValue(item));

            return rowsAffected;
        }

        async Task<IEnumerable<AppUser>> IUserRepository.GetAllAsync()
        {
            var data = await _context.Set<AppUser>()
                                .FromSqlRaw($"GetAll{nameof(AppUser)}")
                                .ToListAsync();

            return data;
        }

        async Task<AppUser> IUserRepository.GetAsync(string email, string password)
        {
            AppUser result = null;
            var data = await _context.Set<AppUser>()
                                .FromSqlRaw($"GetAll{nameof(AppUser)}")
                                .ToListAsync();
            foreach (var item in data)
            {
                if (item.Email == email
                    && item.Password == password)
                {
                    result = item;
                    break;
                }
            }

            return result;
        }

        async Task<AppUser> IUserRepository.GetAsync(string email)
        {
            AppUser result = null;
            var data = await _context.Set<AppUser>()
                                .FromSqlRaw($"GetAll{nameof(AppUser)}")
                                .ToListAsync();
            foreach (var item in data)
            {
                if (item.Email == email)
                {
                    result = item;
                    break;
                }
            }

            return result;
        }

        async Task<AppUser> IUserRepository.GetByIdAsync(string id)
        {
            var data = await Task.Run(() => _context.Set<AppUser>()
                                .FromSqlRaw($"Get{nameof(AppUser)}ById @Id", new SqlParameter("Id", id))
                                .AsEnumerable()
                                .FirstOrDefault());

            return data;
        }

        async Task<int> IUserRepository.UpdateAsync(AppUser item)
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

            var rowsAffected = await _context.Database
                                .ExecuteSqlRawAsync($"Update{item.GetType().Name} {procedureParamNames}", procedureParams);

            return rowsAffected;
        }
    }
}