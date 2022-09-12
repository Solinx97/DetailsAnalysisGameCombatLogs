using CombatAnalysis.DAL.Data;
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

        async Task<int> ITokenRepository.CreateAsync(RefreshToken item)
        {
            await _context.Set<RefreshToken>().AddAsync(item);
            var numberEntries = await _context.SaveChangesAsync();

            return numberEntries;
        }

        async Task<int> ITokenRepository.DeleteAsync(RefreshToken item)
        {
            _context.Set<RefreshToken>().Remove(item);
            var numberEntries = await _context.SaveChangesAsync();

            return numberEntries;
        }

        async Task<RefreshToken> ITokenRepository.Get(string token)
        {
            var allTokens = await _context.Set<RefreshToken>().AsNoTracking().ToListAsync();
            if (!allTokens.Any())
            {
                return null;
            }

            var foundToken = allTokens.Find(refreshToken => refreshToken.Token == token);
            return foundToken;
        }

        async Task<List<RefreshToken>> ITokenRepository.GetByUser(string userId)
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
            var numberEntries = await _context.SaveChangesAsync();

            return numberEntries;
        }
    }
}
