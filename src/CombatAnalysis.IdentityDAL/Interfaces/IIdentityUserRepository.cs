using CombatAnalysis.IdentityDAL.Entities;

namespace CombatAnalysis.IdentityDAL.Interfaces;

public interface IIdentityUserRepository
{
    Task SaveAsync(IdentityUser identityUser);

    IdentityUser Get(string id);

    Task<IdentityUser> GetAsync(string email, string password);
}
