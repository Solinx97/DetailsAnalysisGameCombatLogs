using CombatAnalysis.BL.DTO;
using CombatAnalysis.BL.DTO.Chat;
using CombatAnalysis.BL.Interfaces;
using CombatAnalysis.BL.Services;
using CombatAnalysis.BL.Services.Chat;
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
        services.AddScoped<IService<InviteToGroupChatDto, int>, InviteToGroupChatService>();
        services.AddScoped<IService<GroupChatDto, int>, GroupChatService>();
        services.AddScoped<IService<GroupChatMessageDto, int>, GroupChatMessageService>();
        services.AddScoped<IService<GroupChatUserDto, int>, GroupChatUserService>();

        services.AddScoped<IService<CombatLogDto, int>, CombatLogService>();
        services.AddScoped<IService<CombatLogByUserDto, int>, CombatLogByUserService>();
        services.AddScoped<IService<CombatDto, int>, CombatService>();
        services.AddScoped<IService<CombatPlayerDto, int>, CombatPlayerService>();
        services.AddScoped<IService<DamageDoneDto, int>, DamageDoneService>();
        services.AddScoped<IService<DamageDoneGeneralDto, int>, DamageDoneGeneralService>();
        services.AddScoped<IService<HealDoneDto, int>, HealDoneService>();
        services.AddScoped<IService<HealDoneGeneralDto, int>, HealDoneGeneralService>();
        services.AddScoped<IService<DamageTakenDto, int>, DamageTakenService>();
        services.AddScoped<IService<DamageTakenGeneralDto, int>, DamageTakenGeneralService>();
        services.AddScoped<IService<ResourceRecoveryDto, int>, ResourceRecoveryService>();
        services.AddScoped<IService<ResourceRecoveryGeneralDto, int>, ResourceRecoveryGeneralService>();
    }
}
