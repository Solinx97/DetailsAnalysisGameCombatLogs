using CombatAnalysis.Identity.DTO;

namespace CombatAnalysis.Identity.Interfaces;

public interface IIdentityUserService
{
    Task CreateAsync(IdentityUserDto user);

    Task<IdentityUserDto> GetByIdAsync(string id);

    Task<bool> CheckByEmailAsync(string email);

    Task<IdentityUserDto> GetAsync(string emil);

    Task UpdateAsync(IdentityUserDto user);
}
