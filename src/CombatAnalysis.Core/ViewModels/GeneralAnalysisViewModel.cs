using CombatAnalysis.Core.Commands;
using CombatAnalysis.Core.Consts;
using CombatAnalysis.Core.Interfaces;
using CombatAnalysis.Core.Models;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CombatAnalysis.Core.ViewModels
{
    public class GeneralAnalysisViewModel : MvxViewModel<List<CombatModel>>
    {
        private readonly IMvxNavigationService _mvvmNavigation;
        private readonly IViewModelConnect _handler;

        private MvxViewModel _basicTemplate;
        private List<CombatModel> _combats;
        private int _combatIndex;
        private string _combatStatus = "Победа";

        public GeneralAnalysisViewModel(IMvxNavigationService mvvmNavigation)
        {
            _mvvmNavigation = mvvmNavigation;

            _combats = new List<CombatModel>();
            _handler = new ViewModelMConnect();

            ShowDetailsCommand = new MvxCommand(ShowDetails);

            BasicTemplate = Templates.Basic;
            _handler.PropertyUpdate<BasicTemplateViewModel>(Templates.Basic, "Step", 1);
        }

        public IMvxCommand ShowDetailsCommand { get; set; }

        public MvxViewModel BasicTemplate
        {
            get { return _basicTemplate; }
            set
            {
                SetProperty(ref _basicTemplate, value);
            }
        }

        public List<CombatModel> Combats
        {
            get { return _combats; }
            set
            {
                SetProperty(ref _combats, value);
            }
        }

        public int CombatIndex
        {
            get { return _combatIndex; }
            set
            {
                SetProperty(ref _combatIndex, value);
            }
        }

        public string CombatStatus
        {
            get { return _combatStatus; }
            set
            {
                SetProperty(ref _combatStatus, value);
            }
        }

        public override void Prepare(List<CombatModel> parameter)
        {
            if (parameter != null && parameter.Count > 0)
            {
                Combats = parameter;
            }
        }

        public override void ViewDestroy(bool viewFinishing = true)
        {
            ((BasicTemplateViewModel)Templates.Basic).Combats = Combats;

            base.ViewDestroy(viewFinishing);
        }

        public void ShowDetails()
        {
            Task.Run(() => _mvvmNavigation.Navigate<DetailsSpecificalCombatViewModel, CombatModel>(Combats[CombatIndex]));
        }
    }
}
