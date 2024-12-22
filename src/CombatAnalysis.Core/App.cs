using AutoMapper;
using CombatAnalysis.CombatParser.Core;
using CombatAnalysis.CombatParser.Interfaces;
using CombatAnalysis.CombatParser.Services;
using CombatAnalysis.Core.Consts;
using CombatAnalysis.Core.Core;
using CombatAnalysis.Core.Enums;
using CombatAnalysis.Core.Helpers;
using CombatAnalysis.Core.Interfaces;
using CombatAnalysis.Core.Mapping;
using CombatAnalysis.Core.Services;
using CombatAnalysis.Core.ViewModels.ViewModelTemplates;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using MvvmCross;
using MvvmCross.ViewModels;
using System.Configuration;

namespace CombatAnalysis.Core;

public class App : MvxApplication
{
    public override void Initialize()
    {
        Port.CombatParserApi = ConfigurationManager.AppSettings.Get("combatParserApiPort") ?? string.Empty;
        Port.UserApi = ConfigurationManager.AppSettings.Get("userApiPort") ?? string.Empty;
        Port.ChatApi = ConfigurationManager.AppSettings.Get("chatApiPort") ?? string.Empty;
        Port.Identity = ConfigurationManager.AppSettings.Get("identityPort") ?? string.Empty;

        Authentication.ClientId = ConfigurationManager.AppSettings.Get("clientId") ?? string.Empty;
        Authentication.Scope = ConfigurationManager.AppSettings.Get("scope") ?? string.Empty;
        Authentication.RedirectUri = ConfigurationManager.AppSettings.Get("redirectUri") ?? string.Empty;
        Authentication.Protocol = ConfigurationManager.AppSettings.Get("protocol") ?? string.Empty;
        Authentication.Listener = ConfigurationManager.AppSettings.Get("listener") ?? string.Empty;

        AuthenticationGrantType.Code = ConfigurationManager.AppSettings.Get("grantTypeCode") ?? string.Empty;
        AuthenticationGrantType.Authorization = ConfigurationManager.AppSettings.Get("grantTypeAuthorization") ?? string.Empty;
        AuthenticationGrantType.RefreshToken = ConfigurationManager.AppSettings.Get("grantTypeRefreshToken") ?? string.Empty;

        AppInformation.Version = ConfigurationManager.AppSettings.Get("appVersion") ?? string.Empty;
        if (Enum.TryParse(ConfigurationManager.AppSettings.Get("appVersionType"), out AppVersionType appVersionType))
        {
            AppInformation.VersionType = appVersionType;
        }

        var mappingConfig = new MapperConfiguration(mc =>
        {
            mc.AddProfile(new CombatAnalysisMapper());
        });

        var loggerFactory = new LoggerFactory();
        var logger = new Logger<ILogger>(loggerFactory);
        var mapper = mappingConfig.CreateMapper();

        IHttpClientHelper httpClient = new HttpClientHelper();
        IFileManager fileManager = new FileManager();
        var parser = new CombatParserService(fileManager, logger);

        var memoryCacheOptions = new MemoryCacheOptions { SizeLimit = 2048 };
        var memoryCache = new MemoryCache(memoryCacheOptions);

        var identityService = new IdentityService(memoryCache, httpClient, logger);
        var cacheService = new CacheService();

        Mvx.IoCProvider.RegisterSingleton(mapper);
        Mvx.IoCProvider.RegisterSingleton(httpClient);
        Mvx.IoCProvider.RegisterSingleton(parser);
        Mvx.IoCProvider.RegisterSingleton<ILogger>(logger);
        Mvx.IoCProvider.RegisterSingleton<IMemoryCache>(memoryCache);
        Mvx.IoCProvider.RegisterSingleton<IIdentityService>(identityService);
        Mvx.IoCProvider.RegisterSingleton<ICacheService>(cacheService);

        RegisterAppStart<BasicTemplateViewModel>();
    }
}
