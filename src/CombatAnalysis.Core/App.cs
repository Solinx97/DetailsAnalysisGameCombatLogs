using AutoMapper;
using CombatAnalysis.Core.Consts;
using CombatAnalysis.Core.Core;
using CombatAnalysis.Core.Interfaces;
using CombatAnalysis.Core.Mapping;
using CombatAnalysis.Core.ViewModels;
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

            var mapper = mappingConfig.CreateMapper();
            IHttpClientHelper httpClient = new HttpClientHelper();

            Mvx.IoCProvider.RegisterSingleton(mapper);
            Mvx.IoCProvider.RegisterSingleton(httpClient);

            RegisterAppStart<MainInformationViewModel>();
        }
    }
}
