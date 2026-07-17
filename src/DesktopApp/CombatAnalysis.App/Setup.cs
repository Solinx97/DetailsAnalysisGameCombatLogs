using CombatAnalysis.App.Services;
using CombatAnalysis.Core.Interfaces;
using CombatAnalysis.Core.ViewModels.User;
using Microsoft.Extensions.Logging;
using MvvmCross;
using MvvmCross.Platforms.Wpf.Core;
using MvvmCross.ViewModels;
using Serilog;
using Serilog.Extensions.Logging;
using System.IO;

namespace CombatAnalysis.App;

public class Setup : MvxWpfSetup<Core.App>
{
    protected override void InitializeApp(IMvxApplication app)
    {
        base.InitializeApp(app);

        Mvx.IoCProvider.RegisterType<IAuthWindowService<AuthorizationViewModel>, LoginWindowService>();
        Mvx.IoCProvider.RegisterType<IAuthWindowService<RegistrationViewModel>, RegistrationWindowService>();
    }

    protected override ILoggerFactory? CreateLogFactory()
    {
        var logFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Logs", "log-.txt");

        Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.File(logFilePath, rollingInterval: RollingInterval.Day)
                .CreateLogger();

        return new SerilogLoggerFactory();
    }

    protected override ILoggerProvider? CreateLogProvider()
    {
        return new SerilogLoggerProvider();
    }
}
