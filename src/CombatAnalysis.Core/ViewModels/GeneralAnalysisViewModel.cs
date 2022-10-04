using CombatAnalysis.Core.Consts;
using CombatAnalysis.Core.Enums;
using CombatAnalysis.Core.Interfaces;
using CombatAnalysis.Core.Interfaces.Observers;
using CombatAnalysis.Core.Models;
using CombatAnalysis.Core.Services;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace CombatAnalysis.Core.ViewModels
{
    public class GeneralAnalysisViewModel : MvxViewModel<Tuple<List<CombatModel>, LogType>>, IResponseStatusObserver
    {
        private readonly IMvxNavigationService _mvvmNavigation;
        private readonly CombatParserAPIService _combatParserAPIService;

        private IImprovedMvxViewModel _basicTemplate;
        private ObservableCollection<CombatModel> _combats;
        private CombatModel _selectedCombat;
        private ResponseStatus _status;
        private LogType _logType;

        public GeneralAnalysisViewModel(IMvxNavigationService mvvmNavigation, IHttpClientHelper httpClient, ILogger logger, IMemoryCache memoryCache)
        {
            _mvvmNavigation = mvvmNavigation;

            //_combats = new List<CombatModel>();
            _combatParserAPIService = new CombatParserAPIService(httpClient, logger, memoryCache);

            RepeatSaveCommand = new MvxCommand(RepeatSaveCombatDataDetails);

            BasicTemplate = Templates.Basic;
            BasicTemplate.Parent = this;
            BasicTemplate.Handler.PropertyUpdate<BasicTemplateViewModel>(BasicTemplate, "Step", 1);

            var responseStatusObservable = (IResponseStatusObservable)BasicTemplate;
            responseStatusObservable.AddObserver(this);
        }

        public IMvxCommand RepeatSaveCommand { get; set; }

        public IImprovedMvxViewModel BasicTemplate
        {
            get { return _basicTemplate; }
            set
            {
                SetProperty(ref _basicTemplate, value);
            }
        }

        public ObservableCollection<CombatModel> Combats
        {
            get { return _combats; }
            set
            {
                SetProperty(ref _combats, value);
            }
        }

        public CombatModel SelectedCombat
        {
            get { return _selectedCombat; }
            set
            {
                SetProperty(ref _selectedCombat, value);

                ShowDetails();
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

        public override void Prepare(Tuple<List<CombatModel>, LogType> parameter)
        {
            if (parameter != null)
            {
                Combats = new ObservableCollection<CombatModel>(parameter.Item1);
                _logType = parameter.Item2;
            }
        }

        public override void ViewDestroy(bool viewFinishing = true)
        {
            ((BasicTemplateViewModel)Templates.Basic).Combats = Combats.ToList();

            base.ViewDestroy(viewFinishing);
        }

        public void ShowDetails()
        {
            BasicTemplate.Handler.PropertyUpdate<BasicTemplateViewModel>(BasicTemplate, "TargetCombat", SelectedCombat);

            Task.Run(() => _mvvmNavigation.Close(this));
            Task.Run(() => _mvvmNavigation.Navigate<DetailsSpecificalCombatViewModel, CombatModel>(SelectedCombat));
        }

        public void RepeatSaveCombatDataDetails()
        {
            BasicTemplate.Handler.PropertyUpdate<BasicTemplateViewModel>(BasicTemplate, "ResponseStatus", ResponseStatus.Pending);

            Task.Run(async () =>
            {
                var responseStatus = await _combatParserAPIService.Save(Combats.ToList(), _logType) ? ResponseStatus.Successful : ResponseStatus.Failed;

                BasicTemplate.Handler.PropertyUpdate<BasicTemplateViewModel>(BasicTemplate, "ResponseStatus", responseStatus);
            });
        }

        public void Update(ResponseStatus status)
        {
            ResponseStatus = status;
        }
    }
}
