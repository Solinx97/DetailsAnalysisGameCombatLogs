using CombatAnalysis.Identity.DTO;

namespace CombatAnalysis.Identity.Interfaces;

public interface IIdentityUserService
{
    Task CreateAsync(IdentityUserDto user);
    
    IdentityUserDto Get(string id);

    Task<IdentityUserDto> GetAsync(string emil, string password);
}
