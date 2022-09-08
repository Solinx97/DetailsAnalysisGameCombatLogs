using CombatAnalysis.Identity.Security;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace CombatAnalysis.WebApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            JWTSecret.GenerateAccessSecretKey();
            JWTSecret.GenerateRefreshSecretKey();

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
