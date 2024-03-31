using CombatAnalysis.BL.DTO;
using CombatAnalysis.BL.DTO.Chat;
using CombatAnalysis.BL.DTO.Community;
using CombatAnalysis.BL.DTO.Post;
using CombatAnalysis.BL.Interfaces;
using CombatAnalysis.BL.Services;
using CombatAnalysis.BL.Services.Chat;
using CombatAnalysis.BL.Services.Community;
using CombatAnalysis.BL.Services.Post;
using CombatAnalysis.DAL.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CombatAnalysis.BL.Extensions;

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

        services.AddScoped<IService<CombatLogDto, int>, CombatLogService>();
        services.AddScoped<IService<CombatLogByUserDto, int>, CombatLogByUserService>();
        services.AddScoped<IService<CombatDto, int>, CombatService>();
        services.AddScoped<IService<CombatPlayerDto, int>, CombatPlayerService>();

        services.AddScoped<IPlayerInfoService<DamageDoneDto, int>, DamageDoneService>();
        services.AddScoped<IPlayerInfoService<DamageDoneGeneralDto, int>, DamageDoneGeneralService>();
        services.AddScoped<IPlayerInfoService<HealDoneDto, int>, HealDoneService>();
        services.AddScoped<IPlayerInfoService<HealDoneGeneralDto, int>, HealDoneGeneralService>();
        services.AddScoped<IPlayerInfoService<DamageTakenDto, int>, DamageTakenService>();
        services.AddScoped<IPlayerInfoService<DamageTakenGeneralDto, int>, DamageTakenGeneralService>();
        services.AddScoped<IPlayerInfoService<ResourceRecoveryDto, int>, ResourceRecoveryService>();
        services.AddScoped<IPlayerInfoService<ResourceRecoveryGeneralDto, int>, ResourceRecoveryGeneralService>();
    }
}
