using CombatAnalysis.Core.Commands;
using CombatAnalysis.Core.Consts;
using CombatAnalysis.Core.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using System.Threading.Tasks;

namespace CombatAnalysis.Core.ViewModels
{
    public class HomeViewModel : MvxViewModel
    {
        private readonly IMvxNavigationService _mvvmNavigation;

        private IImprovedMvxViewModel _basicTemplate;
        private IViewModelConnect _handler;

        public HomeViewModel(IMvxNavigationService mvvmNavigation, IHttpClientHelper httpClient, IMemoryCache memoryCache)
        {
            _mvvmNavigation = mvvmNavigation;

            _handler = new ViewModelMConnect();

            OpenChatCommand = new MvxAsyncCommand(OpenChatAsync);
            OpenCombatAnalysisCommand = new MvxAsyncCommand(OpenCombatAnalysisAsync);

            BasicTemplate = new BasicTemplateViewModel(_handler, mvvmNavigation, memoryCache, httpClient);
            BasicTemplate.Handler.PropertyUpdate<BasicTemplateViewModel>(BasicTemplate, "Step", -1);
            Templates.Basic = BasicTemplate;
        }

        #region Command

        public IMvxAsyncCommand OpenChatCommand { get; set; }

        public IMvxAsyncCommand OpenCombatAnalysisCommand { get; set; }

        #endregion

        #region Properties

        public IImprovedMvxViewModel BasicTemplate
        {
            get { return _basicTemplate; }
            set
            {
                SetProperty(ref _basicTemplate, value);
            }
        }

        #endregion

        public async Task OpenChatAsync()
        {
            await _mvvmNavigation.Navigate<ChatViewModel>();
        }

        public async Task OpenCombatAnalysisAsync()
        {
            await _mvvmNavigation.Navigate<CombatLogInformationViewModel>();
        }
    }
}
