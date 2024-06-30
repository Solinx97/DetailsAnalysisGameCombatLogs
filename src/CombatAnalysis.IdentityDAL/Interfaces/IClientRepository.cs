using CombatAnalysis.IdentityDAL.Entities;

namespace CombatAnalysis.IdentityDAL.Interfaces;

public interface IClientRepository
{
    Task SaveAsync(Client identityUser);

    Task<Client> GetByIdAsync(string id);
}
