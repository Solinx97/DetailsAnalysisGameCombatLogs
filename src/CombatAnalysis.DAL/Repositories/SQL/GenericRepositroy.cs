﻿using CombatAnalysis.DAL.Data;
using CombatAnalysis.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CombatAnalysis.DAL.Repositories.SQL
{
    public class GenericRepository<TModel, TIdType> : IGenericRepository<TModel, TIdType>
        where TModel : class
        where TIdType : notnull
    {
        private readonly CombatAnalysisContext _context;

        public GenericRepository(CombatAnalysisContext context)
        {
            _context = context;
        }

        async Task<int> IGenericRepository<TModel, TIdType>.CreateAsync(TModel item)
        {
            var entityEntry = await _context.Set<TModel>().AddAsync(item);
            await _context.SaveChangesAsync();

            var entityId = (int)entityEntry.Property("Id").CurrentValue;

            return entityId;
        }

        async Task<int> IGenericRepository<TModel, TIdType>.DeleteAsync(TModel item)
        {
            _context.Set<TModel>().Remove(item);
            var numberEntries = await _context.SaveChangesAsync();

            return numberEntries;
        }

        async Task<IEnumerable<TModel>> IGenericRepository<TModel, TIdType>.GetAllAsync() => await _context.Set<TModel>().AsNoTracking().ToListAsync();

        async Task<TModel> IGenericRepository<TModel, TIdType>.GetByIdAsync(TIdType id)
        {
            var entity = await _context.Set<TModel>().FindAsync(id);

            if (entity != null)
            {
                _context.Entry(entity).State = EntityState.Detached;
            }

            return entity;
        }

        IEnumerable<TModel> IGenericRepository<TModel, TIdType>.GetByParam(string paramName, object value)
        {
            var collection = _context.Set<TModel>().AsEnumerable();
            var data = collection.Where(x => x.GetType().GetProperty(paramName).GetValue(x).Equals(value));

            return data;
        }

        async Task<int> IGenericRepository<TModel, TIdType>.UpdateAsync(TModel item)
        {
            _context.Entry(item).State = EntityState.Modified;
            var numberEntries = await _context.SaveChangesAsync();

            return numberEntries;
        }
    }
}