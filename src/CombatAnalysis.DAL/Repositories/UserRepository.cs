using CombatAnalysis.DAL.Data;
using CombatAnalysis.DAL.Entities.User;
using CombatAnalysis.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CombatAnalysis.DAL.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly CombatAnalysisContext _context;

        public UserRepository(CombatAnalysisContext context)
        {
            _context = context;
        }

        async Task<string> IUserRepository.CreateAsync(AppUser item)
        {
            var entityEntry = await _context.Set<AppUser>().AddAsync(item);
            await _context.SaveChangesAsync();

            var entityId = (string)entityEntry.Property("Id").CurrentValue;

            return entityId;
        }

        async Task<int> IUserRepository.DeleteAsync(AppUser item)
        {
            _context.Set<AppUser>().Remove(item);
            var numberEntries = await _context.SaveChangesAsync();

            return numberEntries;
        }

        async Task<IEnumerable<AppUser>> IUserRepository.GetAllAsync() => await _context.Set<AppUser>().AsNoTracking().ToListAsync();

        async Task<AppUser> IUserRepository.GetByIdAsync(string id)
        {
            var entity = await _context.Set<AppUser>().FindAsync(id);

            if (entity != null)
            {
                _context.Entry(entity).State = EntityState.Detached;
            }

            return entity;
        }

        async Task<AppUser> IUserRepository.GetAsync(string email, string password)
        {
            var entity = await _context.Set<AppUser>().FirstOrDefaultAsync(x => x.Email == email && x.Password == password);

            if (entity != null)
            {
                _context.Entry(entity).State = EntityState.Detached;
            }

            return entity;
        }

        async Task<AppUser> IUserRepository.GetAsync(string email)
        {
            var entity = await _context.Set<AppUser>().FirstOrDefaultAsync(x => x.Email == email);

            if (entity != null)
            {
                _context.Entry(entity).State = EntityState.Detached;
            }

            return entity;
        }

        async Task<int> IUserRepository.UpdateAsync(AppUser item)
        {
            _context.Entry(item).State = EntityState.Modified;
            var numberEntries = await _context.SaveChangesAsync();

            return numberEntries;
        }
    }
}
