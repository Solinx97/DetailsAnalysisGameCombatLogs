using CombatAnalysis.DAL.Data;
using CombatAnalysis.DAL.Entities.User;
using CombatAnalysis.DAL.Interfaces;
using Firebase.Database.Query;
using System;
using System.Collections.Generic;
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

        async Task<string> IUserRepository.CreateAsync(AppUser item)
        {
            var result = await _context.FirebaseClient
                         .Child(item.GetType().Name)
                         .PostAsync(item);
            return result.Key;
        }

        async Task<int> IUserRepository.DeleteAsync(AppUser item)
        {
            throw new NotImplementedException();
        }

        async Task<IEnumerable<AppUser>> IUserRepository.GetAllAsync()
        {
            throw new NotImplementedException();
        }

        async Task<AppUser> IUserRepository.GetAsync(string email, string password)
        {
            throw new NotImplementedException();
        }

        async Task<AppUser> IUserRepository.GetAsync(string email)
        {
            throw new NotImplementedException();
        }

        async Task<AppUser> IUserRepository.GetByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        async Task<int> IUserRepository.UpdateAsync(AppUser item)
        {
            throw new NotImplementedException();
        }
    }
}
