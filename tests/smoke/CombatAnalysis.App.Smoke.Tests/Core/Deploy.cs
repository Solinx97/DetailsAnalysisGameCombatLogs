using Microsoft.Extensions.Configuration;
using Microsoft.SqlServer.Dac;
using System;
using System.IO;

namespace CombatAnalysis.App.Smoke.Tests.Core
{
    internal static class Deploy
    {
        public static void Run()
        {
            var config = new ConfigurationBuilder()
                 .AddJsonFile(AppDomain.CurrentDomain.BaseDirectory + "appsettings.json")
                 .Build();

            var connection = config.GetConnectionString("TestConnection");

            var baseDirectory = AppDomain.CurrentDomain.BaseDirectory.Split("tests")[0];

            const string dacPacPath = @"tests\CombatAnalysis.DataBase\DacpacData\CombatAnalysis.DataBase.dacpac";
            var path = Path.GetFullPath(Path.Combine(baseDirectory, dacPacPath));
            var dacPack = new DacServices(connection);
            var dacOptions = new DacDeployOptions { CreateNewDatabase = true };

            using var dp = DacPackage.Load(path);
            dacPack.Deploy(dp, "Combat_Analysis_Tests", true, dacOptions);
        }
    }
}
