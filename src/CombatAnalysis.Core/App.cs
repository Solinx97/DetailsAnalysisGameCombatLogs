using AutoMapper;
using CombatAnalysis.CombatParser.Core;
using CombatAnalysis.CombatParser.Entities;
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
using System.Collections;
using System.Configuration;

namespace CombatAnalysis.Core;

public class App : MvxApplication
{
    public override void Initialize()
    {
        Port.CombatParserApi = ConfigurationManager.AppSettings.Get("combatParserApiPort");
        Port.UserApi = ConfigurationManager.AppSettings.Get("userApiPort");
        Port.ChatApi = ConfigurationManager.AppSettings.Get("chatApiPort");

        GetPlayerInfo();

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
        IParser<Combat> parser = new CombatParserService(fileManager, logger);

        var memoryCacheOptions = new MemoryCacheOptions { SizeLimit = 1024 };
        var memoryCache = new MemoryCache(memoryCacheOptions);

        Mvx.IoCProvider.RegisterSingleton(mapper);
        Mvx.IoCProvider.RegisterSingleton(httpClient);
        Mvx.IoCProvider.RegisterSingleton(parser);
        Mvx.IoCProvider.RegisterSingleton<ILogger>(logger);
        Mvx.IoCProvider.RegisterSingleton<IMemoryCache>(memoryCache);

        RegisterAppStart<BasicTemplateViewModel>();
    }

    private static void GetPlayerInfo()
    {
        var specsSection = (Hashtable)ConfigurationManager.GetSection("players/specs");
        var castToSpecsDictionary = specsSection.Cast<DictionaryEntry>()
                 .ToDictionary(n => n.Key.ToString(), n => n.Value.ToString());

        PlayerInfoConfiguration.Specs = castToSpecsDictionary;

        var classesSection = (Hashtable)ConfigurationManager.GetSection("players/classes");
        var castToClassesDictionary = classesSection.Cast<DictionaryEntry>()
                 .ToDictionary(n => n.Key.ToString(), n => n.Value.ToString());

        PlayerInfoConfiguration.Classes = castToClassesDictionary;
    }
}
