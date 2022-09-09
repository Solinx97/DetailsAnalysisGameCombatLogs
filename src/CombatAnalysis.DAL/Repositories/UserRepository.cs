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

        async Task<string> IUserRepository.CreateAsync(User item)
        {
            var entityEntry = await _context.Set<User>().AddAsync(item);
            await _context.SaveChangesAsync();

            var entityId = (string)entityEntry.Property("Id").CurrentValue;

            return entityId;
        }

        async Task<int> IUserRepository.DeleteAsync(User item)
        {
            _context.Set<User>().Remove(item);
            var numberEntries = await _context.SaveChangesAsync();

            return numberEntries;
        }

        async Task<IEnumerable<User>> IUserRepository.GetAllAsync() => await _context.Set<User>().AsNoTracking().ToListAsync();

        async Task<User> IUserRepository.GetByIdAsync(string id)
        {
            var entity = await _context.Set<User>().FindAsync(id);

            if (entity != null)
            {
                _context.Entry(entity).State = EntityState.Detached;
            }

            return entity;
        }

        async Task<User> IUserRepository.GetAsync(string email, string password)
        {
            var entity = await _context.Set<User>().FirstOrDefaultAsync(x => x.Email == email && x.Password == password);

            if (entity != null)
            {
                _context.Entry(entity).State = EntityState.Detached;
            }

            return entity;
        }

        async Task<User> IUserRepository.GetAsync(string email)
        {
            var entity = await _context.Set<User>().FirstOrDefaultAsync(x => x.Email == email);

            if (entity != null)
            {
                _context.Entry(entity).State = EntityState.Detached;
            }

            return entity;
        }

        async Task<int> IUserRepository.UpdateAsync(User item)
        {
            _context.Entry(item).State = EntityState.Modified;
            var numberEntries = await _context.SaveChangesAsync();

            return numberEntries;
        }
    }
}
