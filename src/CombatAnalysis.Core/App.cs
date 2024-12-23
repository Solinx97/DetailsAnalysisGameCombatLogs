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
using CombatAnalysis.Core.Security;
using CombatAnalysis.Core.Services;
using CombatAnalysis.Core.ViewModels.ViewModelTemplates;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MvvmCross;
using MvvmCross.ViewModels;

namespace CombatAnalysis.Core;


public class App : MvxApplication
{
#if DEBUG
    private const string environment = "Development";
#else
    private const string environment = "Production";
#endif

    public IConfiguration Configuration { get; }

    public App()
    {
        var builder = new ConfigurationBuilder()
            .AddJsonFile($"appsettings.json", optional: true, reloadOnChange: true)
            .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true)
            .AddUserSecrets<App>();

        Configuration = builder.Build();
    }

    public override void Initialize()
    {
        SecurityKeys.AESKey = Configuration["EncryptedKey"] ?? string.Empty;
        SecurityKeys.IV = Configuration["EncryptedIV"] ?? string.Empty;

        Port.CombatParserApi = Configuration["ApiPort:CombatParser"] ?? string.Empty;
        Port.UserApi = Configuration["ApiPort:User"] ?? string.Empty;
        Port.ChatApi = Configuration["ApiPort:Chat"] ?? string.Empty;
        Port.Identity = Configuration["ApiPort:Identity"] ?? string.Empty;

        Authentication.ClientId = Configuration["App:Auth:ClientId"] ?? string.Empty;
        Authentication.Scope = Configuration["App:Auth:Scope"] ?? string.Empty;
        Authentication.RedirectUri = Configuration["App:Auth:RedirectUri"] ?? string.Empty;
        Authentication.Protocol = Configuration["App:Auth:Protocol"] ?? string.Empty;
        Authentication.Listener = Configuration["App:Auth:Listener"] ?? string.Empty;

        AuthenticationGrantType.Code = Configuration["App:Auth:GrantType:Code"] ?? string.Empty;
        AuthenticationGrantType.Authorization = Configuration["App:Auth:GrantType:Authorization"] ?? string.Empty;
        AuthenticationGrantType.RefreshToken = Configuration["App:Auth:GrantType:RefreshToken"] ?? string.Empty;

        AppInformation.Version = Configuration["App:Version"] ?? string.Empty;
        if (Enum.TryParse(Configuration["App:VersionType"], out AppVersionType appVersionType))
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
