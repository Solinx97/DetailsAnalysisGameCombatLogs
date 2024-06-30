using CombatAnalysis.Identity.DTO;

namespace CombatAnalysis.Identity.Interfaces;

public interface IIdentityUserService
{
    Task CreateAsync(IdentityUserDto user);

    Task<IdentityUserDto> GetByIdAsync(string id);

    Task<IdentityUserDto> GetAsync(string emil, string password);
}
