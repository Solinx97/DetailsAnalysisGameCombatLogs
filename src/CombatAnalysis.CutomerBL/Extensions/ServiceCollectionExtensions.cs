using CombatAnalysis.CustomerBL.DTO;
using CombatAnalysis.CustomerBL.Interfaces;
using CombatAnalysis.CustomerBL.Services;
using CombatAnalysis.CustomerBL.Services.User;
using CombatAnalysis.CustomerDAL.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CombatAnalysis.CustomerBL.Extensions;

public static class ServiceCollectionExtensions
{
    public static void CustomerBLDependencies(this IServiceCollection services, IConfiguration configuration, string connectionName)
    {
        services.CustomerDALDependencies(configuration, connectionName);

        services.AddScoped<IUserService<AppUserDto>, UserService>();

        services.AddScoped<IService<CustomerDto, string>, CustomerService>();
        services.AddScoped<IService<RequestToConnectDto, int>, RequestToConnectService>();
        services.AddScoped<IService<FriendDto, int>, FriendService>();
        services.AddScoped<IService<BannedUserDto, int>, BannedUserService>();
    }
}
