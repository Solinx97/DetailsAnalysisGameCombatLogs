using CombatAnalysis.CustomerBL.DTO;
using CombatAnalysis.CustomerBL.Interfaces;
using CombatAnalysis.CustomerBL.Services;
using CombatAnalysis.CustomerBL.Services.User;
using CombatAnalysis.DAL.Extensions;
using CombatAnalysis.PostDislikeBL.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CombatAnalysis.CustomerBL.Extensions;

public static class ServiceCollectionExtensions
{
    public static void CustomerBLDependencies(this IServiceCollection services, IConfiguration configuration, string connectionName)
    {
        services.RegisterDependenciesForDAL(configuration, connectionName);

        services.AddScoped<IUserService<AppUserDto>, UserService>();

        services.AddScoped<IService<CustomerDto, string>, CustomerService>();
        services.AddScoped<IService<PostDto, int>, PostService>();
        services.AddScoped<IService<PostLikeDto, int>, PostLikeService>();
        services.AddScoped<IService<PostDislikeDto, int>, PostDislikeService>();
        services.AddScoped<IService<PostCommentDto, int>, PostCommentService>();
        services.AddScoped<IService<RequestToConnectDto, int>, RequestToConnectService>();
        services.AddScoped<IService<FriendDto, int>, FriendService>();
        services.AddScoped<IService<BannedUserDto, int>, BannedUserService>();
    }
}
