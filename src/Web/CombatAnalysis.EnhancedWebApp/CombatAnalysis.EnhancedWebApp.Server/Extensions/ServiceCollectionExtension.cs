using CombatAnalysis.EnhancedWebApp.Server.Handlers;
using CombatAnalysis.EnhancedWebApp.Server.HttpClients;
using CombatAnalysis.EnhancedWebApp.Server.Interfaces.HttpClients;

namespace CombatAnalysis.EnhancedWebApp.Server.Extensions;

internal static class ServiceCollectionExtension
{
    public static void AddApiClients(this IServiceCollection sc, ConfigurationManager cm)
    {
        sc.AddHttpClient<IGroupChatApiClient, GroupChatApiClient>(client =>
        {
            client.BaseAddress = new Uri($"{cm.GetSection("Cluster:Chat").Value ?? ""}api/v1/");
        })
            .AddHttpMessageHandler<AuthorizationHandler>()
            .AddHttpMessageHandler<ApiErrorHandler>();

        sc.AddHttpClient<IPersonalChatApiClient, PersonalChatApiClient>(client =>
        {
            client.BaseAddress = new Uri($"{cm.GetSection("Cluster:Chat").Value ?? ""}api/v1/");
        })
            .AddHttpMessageHandler<AuthorizationHandler>()
            .AddHttpMessageHandler<ApiErrorHandler>();

        sc.AddHttpClient<IVoiceChatApiClient, VoiceChatApiClient>(client =>
        {
            client.BaseAddress = new Uri($"{cm.GetSection("Cluster:Chat").Value ?? ""}api/v1/");
        })
            .AddHttpMessageHandler<AuthorizationHandler>()
            .AddHttpMessageHandler<ApiErrorHandler>();

        sc.AddHttpClient<IWoWAccountGameDataApiClient, WoWAccountGameDataApiClient>(client =>
        {
            client.BaseAddress = new Uri(cm.GetSection("BattleNet:BattleNetAPI").Value ?? "");
        })
            .AddHttpMessageHandler<WoWCharacterGameDataAuthorizationHandler>()
            .AddHttpMessageHandler<ExternalApiErrorHandler>();

        sc.AddHttpClient<IWoWGameDataApiClient, WoWGameDataApiClient>(client =>
        {
            client.BaseAddress = new Uri(cm.GetSection("BattleNet:BattleNetAPI").Value ?? "");
        })
            .AddHttpMessageHandler<WoWGameDataAuthorizationHandler>()
            .AddHttpMessageHandler<ExternalApiErrorHandler>();

        sc.AddHttpClient<IWoWCharacterGameDataApiClient, WoWCharacterGameDataApiClient>(client =>
        {
            client.BaseAddress = new Uri(cm.GetSection("BattleNet:BattleNetAPI").Value ?? "");
        })
            .AddHttpMessageHandler<WoWGameDataAuthorizationHandler>()
            .AddHttpMessageHandler<ExternalApiErrorHandler>();

        sc.AddHttpClient<IWoWGameDataAuthApiClient, WoWGameDataAuthApiClient>(client =>
        {
            client.BaseAddress = new Uri(cm.GetSection("BattleNet:BattleNetAutAPI").Value ?? "");
        })
            .AddHttpMessageHandler<WoWGameDataAuthAuthorizationHandler>()
            .AddHttpMessageHandler<ExternalApiErrorHandler>();
    }
}
