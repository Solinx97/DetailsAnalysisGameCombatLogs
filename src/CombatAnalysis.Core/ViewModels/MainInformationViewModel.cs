using CombatAnalysis.Core.Commands;
using CombatAnalysis.Core.Interfaces;
using CombatAnalysis.WinCore;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using System.Threading.Tasks;

namespace CombatAnalysis.Core.ViewModels
{
    public class MainInformationViewModel : MvxViewModel
    {
        private readonly IMvxNavigationService _mvvmNavigation;

        private string _combatLog;
        private bool _isLoading;
        private string _combatLogPath;
        private MvxViewModel _basicTemplate;
        private IViewModelConnect _handler;

        public MainInformationViewModel(IMvxNavigationService mvvmNavigation)
        {
            _mvvmNavigation = mvvmNavigation;

            GetCombatLogCommand = new MvxCommand(GetCombatLog);
            OpenPlayerAnalysisCommand = new MvxCommand(OpenPlayerAnalysis);

            _handler = new ViewModelMConnect();
            BasicTemplate = new BasicTemplateViewModel(this, _handler, _mvvmNavigation);
        }

        public IMvxCommand GetCombatLogCommand { get; set; }

        public IMvxCommand OpenPlayerAnalysisCommand { get; set; }

        public MvxViewModel BasicTemplate
        {
            get { return _basicTemplate; }
            set
            {
                SetProperty(ref _basicTemplate, value);
            }
        }

        public string CombatLog
        {
            get { return _combatLog; }
            set
            {
                SetProperty(ref _combatLog, value);
            }
        }

        public bool IsLoading
        {
            get { return _isLoading; }
            set
            {
                SetProperty(ref _isLoading, value);
            }
        }

        public void GetCombatLog()
        {
            _combatLogPath = WinHandler.FileOpen();
            var split = _combatLogPath.Split(@"\");
            CombatLog = split[split.Length - 1];
        }

        public void OpenPlayerAnalysis()
        {
            IsLoading = true;
            Task.Run(() => _mvvmNavigation.Navigate<GeneralAnalysisViewModel, string>(_combatLogPath));
        }

        public override void ViewAppeared()
        {
            IsLoading = false;
        }
    }
}
