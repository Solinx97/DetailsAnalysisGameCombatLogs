﻿using CombatAnalysis.Identity.Interfaces;
using CombatAnalysis.Identity.Services;
using CombatAnalysis.IdentityDAL.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace CombatAnalysis.Identity.Extensions;

public static class ServiceCollectionExtensions
{
    public static void RegisterIdentityDependencies(this IServiceCollection services, string connectionString)
    {
        services.RegisterDependenciesForDAL(connectionString);

        services.AddScoped<IIdentityTransactionService, IdentityTransactionService>();

        services.AddScoped<IOAuthCodeFlowService, OAuthCodeFlowService>();
        services.AddScoped<IIdentityUserService, IdentityUserService>();
        services.AddScoped<IClientService, ClientService>();

        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IAuthCodeService, AuthCodeService>();

        services.AddScoped<IUserVerification, UserVerificationService>();
    }
}
