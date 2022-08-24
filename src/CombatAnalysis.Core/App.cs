using AutoMapper;
using CombatAnalysis.CombatParser.Interfaces;
using CombatAnalysis.CombatParser.Services;
using CombatAnalysis.Core.Consts;
using CombatAnalysis.Core.Helpers;
using CombatAnalysis.Core.Interfaces;
using CombatAnalysis.Core.Mapping;
using CombatAnalysis.Core.ViewModels;
using Microsoft.Extensions.Logging;
using MvvmCross;
using MvvmCross.ViewModels;
using System.Configuration;

namespace CombatAnalysis.Core
{
    public class App : MvxApplication
    {
        public override void Initialize()
        {
            Port.CombatParserApi = ConfigurationManager.AppSettings.Get("combatParserApiPort");
            
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new CombatAnalysisMapper());
            });

            var loggerFactory = new LoggerFactory();
            var logger = new Logger<ILogger>(loggerFactory);
            var mapper = mappingConfig.CreateMapper();
            IHttpClientHelper httpClient = new HttpClientHelper();
            ICombatDetails combatDetails = new CombatDetailsService(logger);
            IFileManager fileManager = new FileManager();
            IParser parser = new CombatParserService(combatDetails, fileManager);

            Mvx.IoCProvider.RegisterSingleton(mapper);
            Mvx.IoCProvider.RegisterSingleton(httpClient);
            Mvx.IoCProvider.RegisterSingleton(parser);

            RegisterAppStart<MainInformationViewModel>();
        }
    }
}
