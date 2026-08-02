using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using CombatAnalysis.Core.Core;
using CombatAnalysis.UploadingLogsApp.Consts;
using CombatAnalysis.UploadingLogsApp.Enums;
using CombatAnalysis.UploadingLogsApp.Extensions;
using CombatAnalysis.UploadingLogsApp.Security;
using CombatAnalysis.UploadingLogsApp.ViewModels;
using CombatAnalysis.UploadingLogsApp.Views;
using Microsoft.Extensions.Configuration;
using System;

namespace CombatAnalysis.UploadingLogsApp;

public partial class App : Application
{
#if DEBUG
    private const string environment = "Development";
#else
    private const string environment = "Production";
#endif

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
        AvaloniaXamlLoader.Load(this);

        InitConsts();
    }

    public IConfiguration Configuration { get; }

    public static IServiceProvider Services { get; private set; } = null!;

    public override void OnFrameworkInitializationCompleted()
    {
        Services = ServiceCollectionExtensions.ConfigureServices();

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow
            {
                DataContext = Services.GetService(typeof(MainViewModel)),
            };
        }

        base.OnFrameworkInitializationCompleted();
    }

    private void InitConsts()
    {
        SecurityKeys.AESKey = Configuration["EncryptedKey"] ?? string.Empty;
        SecurityKeys.IV = Configuration["EncryptedIV"] ?? string.Empty;

        API.CombatParserApi = Configuration["API:CombatParser"] ?? string.Empty;
        API.UserApi = Configuration["API:User"] ?? string.Empty;
        API.Identity = Configuration["API:Identity"] ?? string.Empty;

        Authentication.ClientId = Configuration["App:Auth:ClientId"] ?? string.Empty;
        Authentication.Scopes = Configuration["App:Auth:Scopes"] ?? string.Empty;
        Authentication.RedirectUri = Configuration["App:Auth:RedirectUri"] ?? string.Empty;
        Authentication.CancelUri = Configuration["App:Auth:CancelUri"] ?? string.Empty;
        Authentication.Listener = Configuration["App:Auth:Listener"] ?? string.Empty;

        AuthenticationGrantType.Code = Configuration["App:Auth:GrantType:Code"] ?? string.Empty;

        AppInformation.Name = Configuration["App:Name"] ?? string.Empty;
        AppInformation.Version = Configuration["App:Version"] ?? string.Empty;
        if (Enum.TryParse(Configuration["App:VersionType"], out AppVersionType appVersionType))
        {
            AppInformation.VersionType = appVersionType;
        }
    }
}