﻿using CombatAnalysisIdentity.Models;

namespace CombatAnalysisIdentity.Interfaces;

public interface IUserAuthorizationService
{
    Task<string> AuthorizationAsync(HttpRequest request, string email, string password);

    Task<bool> ClientValidationAsync(HttpRequest request);

    Task<bool> CreateUserAsync(IdentityUserModel identityUser, AppUserModel appUser, CustomerModel customer);
}