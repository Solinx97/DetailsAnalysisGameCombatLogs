using CombatAnalysis.Core.Consts;
using CombatAnalysis.Core.Core;
using CombatAnalysis.Core.Interfaces;
using CombatAnalysis.Core.Interfaces.Observers;
using CombatAnalysis.Core.Models;
using CombatAnalysis.Core.Services;
using Microsoft.Extensions.Logging;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CombatAnalysis.Core.ViewModels
{
    public class GeneralAnalysisViewModel : MvxViewModel<List<CombatModel>>, IResponseStatusObserver
    {
        private readonly IMvxNavigationService _mvvmNavigation;
        private readonly CombatParserAPIService _combatParserAPIService;

        private IImprovedMvxViewModel _basicTemplate;
        private List<CombatModel> _combats;
        private int _combatIndex;
        private ResponseStatus _status;

        public GeneralAnalysisViewModel(IMvxNavigationService mvvmNavigation, IHttpClientHelper httpClient, ILogger logger)
        {
            _mvvmNavigation = mvvmNavigation;

            _combats = new List<CombatModel>();
            _combatParserAPIService = new CombatParserAPIService(httpClient, logger);

            ShowDetailsCommand = new MvxCommand(ShowDetails);
            RepeatSaveCommand = new MvxCommand(RepeatSaveCombatDataDetails);

            BasicTemplate = Templates.Basic;
            BasicTemplate.Handler.PropertyUpdate<BasicTemplateViewModel>(BasicTemplate, "Step", 1);

            var responseStatusObservable = (IResponseStatusObservable)BasicTemplate;
            responseStatusObservable.AddObserver(this);
        }

        public IMvxCommand ShowDetailsCommand { get; set; }

        public IMvxCommand RepeatSaveCommand { get; set; }

        public IImprovedMvxViewModel BasicTemplate
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

        public ResponseStatus ResponseStatus
        {
            get { return _status; }
            set
            {
                SetProperty(ref _status, value);
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
            BasicTemplate.Handler.PropertyUpdate<BasicTemplateViewModel>(BasicTemplate, "TargetCombat", Combats[CombatIndex]);

            Task.Run(() => _mvvmNavigation.Navigate<DetailsSpecificalCombatViewModel, CombatModel>(Combats[CombatIndex]));
        }

        public void RepeatSaveCombatDataDetails()
        {
            BasicTemplate.Handler.PropertyUpdate<BasicTemplateViewModel>(BasicTemplate, "ResponseStatus", ResponseStatus.Pending);

            Task.Run(async () =>
            {
                var responseStatus = await _combatParserAPIService.Save(Combats) ? ResponseStatus.Successful : ResponseStatus.Failed;

                BasicTemplate.Handler.PropertyUpdate<BasicTemplateViewModel>(BasicTemplate, "ResponseStatus", responseStatus);
            });
        }

        public void Update(ResponseStatus status)
        {
            ResponseStatus = status;
        }
    }
}
