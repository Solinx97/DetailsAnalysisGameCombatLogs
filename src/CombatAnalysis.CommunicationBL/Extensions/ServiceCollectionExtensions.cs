using CombatAnalysis.CommunicationBL.DTO.Chat;
using CombatAnalysis.CommunicationBL.DTO.Community;
using CombatAnalysis.CommunicationBL.DTO.Post;
using CombatAnalysis.CommunicationBL.Interfaces;
using CombatAnalysis.CommunicationBL.Services;
using CombatAnalysis.CommunicationBL.Services.Chat;
using CombatAnalysis.CommunicationBL.Services.Community;
using CombatAnalysis.CommunicationBL.Services.Post;
using CombatAnalysis.CommunicationDAL.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CombatAnalysis.CommunicationBL.Extensions;

public static class ServiceCollectionExtensions
{
    public static void BLDependencies(this IServiceCollection services, IConfiguration configuration, string connectionName)
    {
        services.RegisterDependenciesForDAL(configuration, connectionName);

        services.AddScoped<ISqlContextService, SqlContextService>();

        services.AddScoped<IService<PersonalChatDto, int>, PersonalChatService>();
        services.AddScoped<IService<PersonalChatMessageDto, int>, PersonalChatMessageService>();
        services.AddScoped<IService<PersonalChatMessageCountDto, int>, PersonalChatMessageCountService>();
        services.AddScoped<IService<GroupChatDto, int>, GroupChatService>();
        services.AddScoped<IService<GroupChatRulesDto, int>, GroupChatRulesService>();
        services.AddScoped<IService<GroupChatMessageDto, int>, GroupChatMessageService>();
        services.AddScoped<IService<UnreadGroupChatMessageDto, int>, UnreadGroupChatMessageService>();
        services.AddScoped<IService<GroupChatMessageCountDto, int>, GroupChatMessageCountService>();
        services.AddScoped<IServiceTransaction<GroupChatUserDto, string>, GroupChatUserService>();

        services.AddScoped<IService<CommunityDto, int>, CommunityService>();
        services.AddScoped<IService<CommunityDiscussionDto, int>, CommunityDiscussionService>();
        services.AddScoped<IService<CommunityDiscussionCommentDto, int>, CommunityDiscussionCommentService>();
        services.AddScoped<IService<CommunityUserDto, string>, CommunityUserService>();
        services.AddScoped<IService<InviteToCommunityDto, int>, InviteToCommunityService>();

        services.AddScoped<IService<PostDto, int>, PostService>();
        services.AddScoped<IService<PostCommentDto, int>, PostCommentService>();
        services.AddScoped<IService<PostLikeDto, int>, PostLikeService>();
        services.AddScoped<IService<PostDislikeDto, int>, PostDislikeService>();
        services.AddScoped<IService<CommunityPostDto, int>, CommunityPostService>();
        services.AddScoped<IService<UserPostDto, int>, UserPostService>();
    }
}
