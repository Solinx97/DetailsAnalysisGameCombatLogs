using CombatAnalysis.DAL.Data.SQL;
using CombatAnalysis.DAL.Entities.Authentication;
using CombatAnalysis.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CombatAnalysis.DAL.Repositories.SQL
{
    public class SQLTokenRepository : ITokenRepository
    {
        private readonly SQLContext _context;

        public SQLTokenRepository(SQLContext context)
        {
            _context = context;
        }

        async Task<RefreshToken> ITokenRepository.CreateAsync(RefreshToken item)
        {
            var entityEntry = await _context.Set<RefreshToken>().AddAsync(item);
            await _context.SaveChangesAsync();

            return entityEntry.Entity;
        }

        async Task<int> ITokenRepository.DeleteAsync(RefreshToken item)
        {
            _context.Set<RefreshToken>().Remove(item);
            var rowsAffected = await _context.SaveChangesAsync();

            return rowsAffected;
        }

        async Task<RefreshToken> ITokenRepository.GetByTokenAsync(string token)
        {
            var allTokens = await _context.Set<RefreshToken>().AsNoTracking().ToListAsync();
            if (!allTokens.Any())
            {
                return null;
            }

            var foundToken = allTokens.Find(refreshToken => refreshToken.Token == token);
            return foundToken;
        }

        async Task<IEnumerable<RefreshToken>> ITokenRepository.GetAllByUserAsync(string userId)
        {
            var allTokens = await _context.Set<RefreshToken>().AsNoTracking().ToListAsync();
            if (!allTokens.Any())
            {
                return null;
            }

            var foundTokens = allTokens.FindAll(refreshToken => refreshToken.UserId == userId);
            return foundTokens;
        }

        async Task<int> ITokenRepository.UpdateAsync(RefreshToken item)
        {
            _context.Entry(item).State = EntityState.Modified;
            var rowsAffected = await _context.SaveChangesAsync();

            return rowsAffected;
        }
    }
}
