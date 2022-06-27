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
using System.Collections.ObjectModel;
using System.Linq;
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
        private bool _isNeedSave = true;
        private bool _isShowSteps;
        private string _foundCombat;
        private string _combatLogPath;
        private int _selectedCombatLogId;
        private MvxViewModel _basicTemplate;
        private IViewModelConnect _handler;
        private List<CombatModel> _combats;
        private ObservableCollection<CombatLogModel> _combatLogs;

        public MainInformationViewModel(IMapper mapper, IMvxNavigationService mvvmNavigation, IHttpClientHelper httpClient)
        {
            _mapper = mapper;
            _mvvmNavigation = mvvmNavigation;

            GetCombatLogCommand = new MvxCommand(GetCombatLog);
            OpenPlayerAnalysisCommand = new MvxCommand(OpenPlayerAnalysis);
            LoadCombatsCommand = new MvxCommand(LoadCombats);

            _combats = new List<CombatModel>();
            _combatParserAPIService = new CombatParserAPIService(mapper, httpClient);

            _handler = new ViewModelMConnect();
            BasicTemplate = new BasicTemplateViewModel(this, _handler, _mvvmNavigation);
        }

        public IMvxCommand GetCombatLogCommand { get; set; }

        public IMvxCommand LoadCombatsCommand { get; set; }

        public IMvxCommand OpenPlayerAnalysisCommand { get; set; }

        public MvxViewModel BasicTemplate
        {
            get { return _basicTemplate; }
            set
            {
                SetProperty(ref _basicTemplate, value);
            }
        }

        public ObservableCollection<CombatLogModel> CombatLogs
        {
            get { return _combatLogs; }
            set
            {
                SetProperty(ref _combatLogs, value);
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

        public int SelectedCombatLogId
        {
            get { return _selectedCombatLogId; }
            set
            {
                SetProperty(ref _selectedCombatLogId, value);
            }
        }

        public void GetCombatLog()
        {
            _combatLogPath = WinHandler.FileOpen();
            var split = _combatLogPath.Split(@"\");
            CombatLog = split[split.Length - 1];
        }

        public void LoadCombatLogs()
        {
            var combatLog = Task.Run(() => LoadCombatLogsAsync());
        }

        public void LoadCombats()
        {
            var combatLog = Task.Run(() => LoadCombatsAsync());
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

            LoadCombatLogs();
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
                createdCombatLogId = await _combatParserAPIService.SaveCombatLog();
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

        private async Task LoadCombatLogsAsync()
        {
            var combatLogsData = await _combatParserAPIService.LoadCombatLogs();
            CombatLogs = new ObservableCollection<CombatLogModel>(combatLogsData);
        }

        private async Task LoadCombatsAsync()
        {
            var id = CombatLogs[SelectedCombatLogId].Id;
            var loadedCombats = await _combatParserAPIService.LoadCombats(id);

            foreach (var item in loadedCombats)
            {
                var players = await _combatParserAPIService.LoadCombatPlayers(item.Id);
                item.Players = players.ToList();
            }

            await _mvvmNavigation.Navigate<GeneralAnalysisViewModel, List<CombatModel>>(loadedCombats.ToList());
        }
    }
}
