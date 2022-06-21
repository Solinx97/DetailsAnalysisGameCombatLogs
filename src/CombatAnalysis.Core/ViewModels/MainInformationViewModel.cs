using AutoMapper;
using CombatAnalysis.CombatParser;
using CombatAnalysis.CombatParser.Interfaces;
using CombatAnalysis.Core.Commands;
using CombatAnalysis.Core.Interfaces;
using CombatAnalysis.Core.Models;
using CombatAnalysis.Core.Services;
using CombatAnalysis.WinCore;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CombatAnalysis.Core.ViewModels
{
    public class MainInformationViewModel : MvxViewModel, IObserver
    {
        private readonly IMvxNavigationService _mvvmNavigation;
        private readonly IMapper _mapper;
        private readonly CombatParserAPIService _combatParserAPIService;

        private string _combatLog;
        private bool _isParsing;
        private bool _isSaving;
        private bool _isNeedSave;
        private bool _isShowSteps;
        private string _foundCombat;
        private string _combatLogPath;
        private MvxViewModel _basicTemplate;
        private IViewModelConnect _handler;
        private List<CombatModel> _combats;

        public MainInformationViewModel(IMapper mapper, IMvxNavigationService mvvmNavigation, IHttpClientHelper httpClient)
        {
            _mapper = mapper;
            _mvvmNavigation = mvvmNavigation;

            IsNeedSave = true;

            GetCombatLogCommand = new MvxCommand(GetCombatLog);
            OpenPlayerAnalysisCommand = new MvxCommand(OpenPlayerAnalysis);

            _combats = new List<CombatModel>();
            _combatParserAPIService = new CombatParserAPIService(mapper, httpClient);

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

        public bool IsParsing
        {
            get { return _isParsing; }
            set
            {
                SetProperty(ref _isParsing, value);
            }
        }

        public bool IsSaving
        {
            get { return _isSaving; }
            set
            {
                SetProperty(ref _isSaving, value);
            }
        }

        public bool IsNeedSave
        {
            get { return _isNeedSave; }
            set
            {
                SetProperty(ref _isNeedSave, value);
            }
        }

        public bool IsShowSteps
        {
            get { return _isShowSteps; }
            set
            {
                SetProperty(ref _isShowSteps, value);
            }
        }

        public string FoundCombat
        {
            get { return _foundCombat; }
            set
            {
                SetProperty(ref _foundCombat, value);
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
            IsParsing = true;

            Task.Run(() => GetData(_combatLogPath));
        }

        public void Update(string combatInformation)
        {
            FoundCombat = combatInformation;
        }

        public override void ViewAppeared()
        {
            IsParsing = false;
            IsSaving = false;
        }

        private async Task GetData(string combatLog)
        {
            var parser = new CombaInformationtParser();
            parser.AddObserver(this);
            await parser.Parse(combatLog);

            var combats = parser.Combats;
            var combatsMapper = _mapper.Map<List<CombatModel>>(combats);
            _combats = combatsMapper;

            await GetDetails();
        }

        private async Task GetDetails()
        {
            var createdCombatLogId = 0;

            if (IsNeedSave)
            {
                IsSaving = true;

                _combatParserAPIService.SetCombats(_combats);
                createdCombatLogId = await _combatParserAPIService.SaveCombatLogData();
            }

            for (int i = 0; i < _combats.Count; i++)
            {
                foreach (var item in _combats[i].Players)
                {
                    _combats[i].DamageDone += item.DamageDone;
                    _combats[i].HealDone += item.HealDone;
                    _combats[i].EnergyRecovery += item.EnergyRecovery;
                    _combats[i].DamageTaken += item.DamageTaken;
                }

                if (IsNeedSave)
                {
                    await _combatParserAPIService.SaveCombatData(_combats[i], createdCombatLogId);
                }
            }

            await _mvvmNavigation.Navigate<GeneralAnalysisViewModel, List<CombatModel>>(_combats);
        }
    }
}
