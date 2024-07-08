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
using CombatAnalysis.Core.ViewModels;
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
        Port.CombatParserApi = ConfigurationManager.AppSettings.Get("combatParserApiPort");
        Port.UserApi = ConfigurationManager.AppSettings.Get("userApiPort");
        Port.ChatApi = ConfigurationManager.AppSettings.Get("chatApiPort");
        Port.Identity = ConfigurationManager.AppSettings.Get("identityPort");
        Authentication.ClientId = ConfigurationManager.AppSettings.Get("clientId");
        Authentication.Scope = ConfigurationManager.AppSettings.Get("scope");
        Authentication.RedirectUri = ConfigurationManager.AppSettings.Get("redirectUri");
        Authentication.Protocol = ConfigurationManager.AppSettings.Get("protocol");
        Authentication.Listener = ConfigurationManager.AppSettings.Get("listener");

        var maxDegreeOfParallelism = ConfigurationManager.AppSettings.Get("maxDegreeOfParallelism");
        if (!string.IsNullOrEmpty(maxDegreeOfParallelism))
        {
            if (int.TryParse(maxDegreeOfParallelism, out var maxDegreeOfParallelismValue))
            {
                ParallelismHelp.MaxDegreeOfParallelism = maxDegreeOfParallelismValue;
            }
        }

        AppInformation.Version = ConfigurationManager.AppSettings.Get("appVersion");
        Enum.TryParse(ConfigurationManager.AppSettings.Get("appVersionType"), out AppVersionType appVersionType);
        AppInformation.VersionType = appVersionType;

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

        Mvx.IoCProvider.RegisterSingleton(mapper);
        Mvx.IoCProvider.RegisterSingleton(httpClient);
        Mvx.IoCProvider.RegisterSingleton(parser);
        Mvx.IoCProvider.RegisterSingleton<ILogger>(logger);
        Mvx.IoCProvider.RegisterSingleton<IMemoryCache>(memoryCache);

        RegisterAppStart<BasicTemplateViewModel>();
    }
}
