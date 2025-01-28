using CombatAnalysis.CommunicationBL.DTO.Community;
using CombatAnalysis.CommunicationBL.DTO.Post;
using CombatAnalysis.CommunicationBL.Interfaces;
using CombatAnalysis.CommunicationBL.Services;
using CombatAnalysis.CommunicationBL.Services.Community;
using CombatAnalysis.CommunicationBL.Services.Post;
using CombatAnalysis.CommunicationDAL.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace CombatAnalysis.CommunicationBL.Extensions;

public static class ServiceCollectionExtensions
{
    public static void CommunicationBLDependencies(this IServiceCollection services, string databaseName, string dataProcessingType, string connectionString)
    {
        services.RegisterDependenciesForDAL(databaseName, dataProcessingType, connectionString);

        services.AddScoped<ISqlContextService, SqlContextService>();

        services.AddScoped<ICommunityService, CommunityService>();
        services.AddScoped<IService<CommunityDiscussionDto, int>, CommunityDiscussionService>();
        services.AddScoped<IService<CommunityDiscussionCommentDto, int>, CommunityDiscussionCommentService>();
        services.AddScoped<IService<CommunityUserDto, string>, CommunityUserService>();
        services.AddScoped<IService<InviteToCommunityDto, int>, InviteToCommunityService>();

        services.AddScoped<IUserPostService, UserPostService>();
        services.AddScoped<IService<UserPostCommentDto, int>, UserPostCommentService>();
        services.AddScoped<IService<UserPostLikeDto, int>, UserPostLikeService>();
        services.AddScoped<IService<UserPostDislikeDto, int>, UserPostDislikeService>();
        services.AddScoped<ICommunityPostService, CommunityPostService>();
        services.AddScoped<IService<CommunityPostCommentDto, int>, CommunityPostCommentService>();
        services.AddScoped<IService<CommunityPostLikeDto, int>, CommunityPostLikeService>();
        services.AddScoped<IService<CommunityPostDislikeDto, int>, CommunityPostDislikeService>();
    }
}
