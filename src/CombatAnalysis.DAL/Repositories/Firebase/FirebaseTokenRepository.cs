using CombatAnalysis.DAL.Data;
using CombatAnalysis.DAL.Entities.Authentication;
using CombatAnalysis.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CombatAnalysis.DAL.Repositories.Firebase
{
    public class FirebaseTokenRepository : ITokenRepository
    {
        private readonly FirebaseContext _context;

        public FirebaseTokenRepository(FirebaseContext context)
        {
            _context = context;
        }

        public Task<int> CreateAsync(RefreshToken item)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAsync(RefreshToken item)
        {
            throw new NotImplementedException();
        }

        public Task<RefreshToken> Get(string token)
        {
            throw new NotImplementedException();
        }

        public Task<List<RefreshToken>> GetByUser(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<int> UpdateAsync(RefreshToken item)
        {
            throw new NotImplementedException();
        }
    }
}
