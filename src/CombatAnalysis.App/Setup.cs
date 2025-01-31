using Microsoft.Extensions.Logging;
using MvvmCross.Platforms.Wpf.Core;
using Serilog;
using Serilog.Extensions.Logging;
using System.IO;

namespace CombatAnalysis.App;

public class Setup : MvxWpfSetup<Core.App>
{
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
