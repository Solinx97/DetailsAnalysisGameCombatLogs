using CombatAnalysis.ChatApi.Core;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace CombatAnalysis.ChatApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    if (args.Length > 0 && args[0] == nameof(CommandLineArgs.Tests))
                    {
                        webBuilder.UseStartup<TestStartup>();
                    }
                    else
                    {
                        webBuilder.UseStartup<Startup>();
                    }
                });
    }
}
