using CombatAnalysis.UserBL.DTO;
using CombatAnalysis.UserBL.Interfaces;
using CombatAnalysis.UserBL.Services;
using CombatAnalysis.UserDAL.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace CombatAnalysis.UserBL.Extensions;

public static class ServiceCollectionExtensions
{
    public static void UserBLDependencies(this IServiceCollection services, string databaseName, string dataProcessingType, string connectionString)
    {
        services.UserDALDependencies(databaseName, dataProcessingType, connectionString);

        services.AddScoped<ICustomerTransactionService, CustomerTransactionService>();

        services.AddScoped<IUserService<AppUserDto>, UserService>();

        services.AddScoped<IService<CustomerDto, string>, CustomerService>();
        services.AddScoped<IService<RequestToConnectDto, int>, RequestToConnectService>();
        services.AddScoped<IService<FriendDto, int>, FriendService>();
        services.AddScoped<IService<BannedUserDto, int>, BannedUserService>();
    }
}
