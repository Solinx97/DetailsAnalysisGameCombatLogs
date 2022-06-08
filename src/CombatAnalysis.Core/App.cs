using AutoMapper;
using CombatAnalysis.Core.Mapper;
using CombatAnalysis.Core.ViewModels;
using MvvmCross;
using MvvmCross.ViewModels;

namespace CombatAnalysis.Core
{
    public class App : MvxApplication
    {
        public override void Initialize()
        {
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new CombatAnalysisMapper());
            });

            IMapper mapper = mappingConfig.CreateMapper();

            Mvx.IoCProvider.RegisterSingleton(mapper);

            RegisterAppStart<MainInformationViewModel>();
        }
    }
}
