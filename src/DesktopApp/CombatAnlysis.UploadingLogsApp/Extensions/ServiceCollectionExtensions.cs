using CombatAnalysis.CombatParser.Core;
using CombatAnalysis.CombatParser.Extensions;
using CombatAnalysis.CombatParser.Interfaces;
using CombatAnalysis.UploadingLogsApp.Core;
using CombatAnalysis.UploadingLogsApp.Helpers;
using CombatAnalysis.UploadingLogsApp.Interfaces;
using CombatAnalysis.UploadingLogsApp.Interfaces.Security;
using CombatAnalysis.UploadingLogsApp.Mapping;
using CombatAnalysis.UploadingLogsApp.Security;
using CombatAnalysis.UploadingLogsApp.Services;
using CombatAnalysis.UploadingLogsApp.ViewModels;
using CombatAnalysis.UploadingLogsApp.ViewModels.User;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace CombatAnalysis.UploadingLogsApp.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceProvider ConfigureServices()
    {
        var services = new ServiceCollection();

        services.AddSingleton<NavigationStore>();
        services.AddSingleton<AppState>();

        services.CombatParser();


        services.AddSingleton<IFileDialogService, FileDialogService>();
        services.AddSingleton<INavigationService, NavigationService>();

        services.AddScoped<IFileManager, FileManager>();
        services.AddScoped<Interfaces.IHttpClientHelper, HttpClientHelper>();

        services.AddMemoryCache();
        services.AddLogging(builder =>
        {
            builder.AddConsole();
        });

        services.AddAutoMapper(cfg =>
        {
            cfg.AddProfile<CombatAnalysisMapper>();
        });

        services.AddScoped<ISecurityStorage, SecurityStorage>();
        services.AddScoped<IIdentityService, IdentityService>();
        services.AddScoped<ICombatParserAPIService, CombatParserAPIService>();

        services.AddSingleton<LoginViewModel>();
        services.AddSingleton<MainViewModel>();
        services.AddSingleton<CombatLogsViewModel>();
        services.AddSingleton<ParsingCombatLogsViewModel>();

        var provider = services.BuildServiceProvider();

        var memoryCache = provider.GetService<IMemoryCache>();
        HttpClientHelperExtensions.Initialize(memoryCache);

        return provider;
    }
}
