﻿using CombatAnalysis.IdentityDAL.Entities;

namespace CombatAnalysis.IdentityDAL.Interfaces;

public interface IIdentityUserRepository
{
    Task SaveAsync(IdentityUser identityUser);

    Task<IdentityUser> GetByIdAsync(string id);

    Task<bool> CheckByEmailAsync(string email);

    Task<IdentityUser> GetAsync(string email);

    Task UpdateAsync(IdentityUser identityUser);
}
