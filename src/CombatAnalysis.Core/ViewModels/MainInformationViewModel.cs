using AutoMapper;
using CombatAnalysis.CombatParser.Interfaces;
using CombatAnalysis.Core.Commands;
using CombatAnalysis.Core.Consts;
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
using System.Net.Http;
using System.Threading.Tasks;

namespace CombatAnalysis.Core.ViewModels
{
    public class MainInformationViewModel : MvxViewModel, IObserver
    {
        private readonly IMvxNavigationService _mvvmNavigation;
        private readonly IMapper _mapper;
        private readonly IParser _parser;
        private readonly CombatParserAPIService _combatParserAPIService;

        private string _combatLog;
        private bool _fileIsNotCorrect;
        private bool _isParsing;
        private bool _isNeedSave = true;
        private bool _isShowSteps;
        private string _foundCombat;
        private string _combatLogPath;
        private int _selectedCombatLogId;
        private int _combatLogsNumber;
        private MvxViewModel _basicTemplate;
        private IViewModelConnect _handler;
        private ObservableCollection<CombatLogModel> _combatLogs;
        private double _screenWidth;
        private double _screenHeight;

        public MainInformationViewModel(IMapper mapper, IMvxNavigationService mvvmNavigation, IHttpClientHelper httpClient, IParser parser)
        {
            _mapper = mapper;
            _mvvmNavigation = mvvmNavigation;
            _parser = parser;

            GetCombatLogCommand = new MvxCommand(GetCombatLog);
            OpenPlayerAnalysisCommand = new MvxCommand(OpenPlayerAnalysis);
            LoadCombatsCommand = new MvxCommand(LoadCombats);
            ReloadCombatsCommand = new MvxCommand(LoadCombatLogs);
            DeleteCombatCommand = new MvxCommand(DeleteCombat);

            _combatParserAPIService = new CombatParserAPIService(httpClient);

            _handler = new ViewModelMConnect();

            BasicTemplate = new BasicTemplateViewModel(this, _handler, mvvmNavigation);
            Templates.Basic = BasicTemplate;
        }

        public IMvxCommand GetCombatLogCommand { get; set; }

        public IMvxCommand LoadCombatsCommand { get; set; }

        public IMvxCommand ReloadCombatsCommand { get; set; }

        public IMvxCommand DeleteCombatCommand { get; set; }

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

        public bool FileIsNotCorrect
        {
            get { return _fileIsNotCorrect; }
            set
            {
                SetProperty(ref _fileIsNotCorrect, value);
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

        public int CombatLogsNumber
        {
            get { return _combatLogsNumber; }
            set
            {
                SetProperty(ref _combatLogsNumber, value);
            }
        }

        public double ScreenWidth
        {
            get { return _screenWidth; }
            set
            {
                SetProperty(ref _screenWidth, value);
            }
        }

        public double ScreenHeight
        {
            get { return _screenHeight; }
            set
            {
                SetProperty(ref _screenHeight, value);
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
            Task.Run(() => LoadCombatLogsAsync());
        }

        public void LoadCombats()
        {
            Task.Run(() => LoadCombatsAsync());
        }

        public void DeleteCombat()
        {
            FoundCombat = string.Empty;
            IsParsing = true;

            Task.Run(() => DeleteAsync());
        }

        public void OpenPlayerAnalysis()
        {
            Task.Run(() => GetCombatDataDetails(_combatLogPath));
        }

        public void Update(string combatInformation)
        {
            FoundCombat = combatInformation;
        }

        public override void ViewAppeared()
        {
            IsParsing = false;

            CombatLogs?.Clear();
            LoadCombatLogs();

            ScreenWidth = System.Windows.SystemParameters.PrimaryScreenWidth * 0.75;
            ScreenHeight = System.Windows.SystemParameters.PrimaryScreenHeight * 0.75;
        }

        private async Task GetCombatDataDetails(string combatLog)
        {
            _parser.AddObserver(this);
            FileIsNotCorrect = !await _parser.FileCheck(combatLog);
            
            if (!FileIsNotCorrect)
            {
                IsParsing = true;

                await _parser.Parse(combatLog);

                var map = _parser.Combats;
                var combats = _mapper.Map<List<CombatModel>>(map);

                _handler.PropertyUpdate<BasicTemplateViewModel>(Templates.Basic, "AllowStep", 1);

                await _mvvmNavigation.Navigate<GeneralAnalysisViewModel, List<CombatModel>>(combats);

                _handler.PropertyUpdate<BasicTemplateViewModel>(Templates.Basic, "Combats", combats);

                if (IsNeedSave)
                {
                    await SaveCombatDataDetails(combats);
                }
            }
        }

        private async Task SaveCombatDataDetails(List<CombatModel> combats)
        {
            try
            {
                _combatParserAPIService.SetCombats(combats);
                var createdCombatLogId = await _combatParserAPIService.SaveCombatLogAsync();
                var tasks = new List<Task>();

                foreach (var item in combats)
                {
                    tasks.Add(_combatParserAPIService.SaveCombatDataAsync(item, createdCombatLogId));
                }

                await Task.WhenAll(tasks);
                await _combatParserAPIService.SetReadyForCombatLog(createdCombatLogId);
            }
            catch (HttpRequestException ex)
            {
                ServerLoadStatus.IsFailed = true;
                _handler.PropertyUpdate<BasicTemplateViewModel>(Templates.Basic, "ServerStatusIsFailed", ServerLoadStatus.IsFailed);
            }
        }

        private async Task LoadCombatLogsAsync()
        {
            var combatLogsData = await _combatParserAPIService.LoadCombatLogsAsync();
            var readyCombatLogData = new List<CombatLogModel>();

            foreach (var item in combatLogsData)
            {
                if (item.IsReady)
                {
                    readyCombatLogData.Add(item);
                }
            }

            CombatLogs = new ObservableCollection<CombatLogModel>(readyCombatLogData);
            CombatLogsNumber = CombatLogs.Count;
        }

        private async Task LoadCombatsAsync()
        {
            var id = CombatLogs[SelectedCombatLogId].Id;
            var loadedCombats = await _combatParserAPIService.LoadCombatsAsync(id);

            foreach (var item in loadedCombats)
            {
                var players = await _combatParserAPIService.LoadCombatPlayersAsync(item.Id);
                item.Players = players.ToList();
            }

            _handler.PropertyUpdate<BasicTemplateViewModel>(Templates.Basic, "AllowStep", 1);
            
            await _mvvmNavigation.Navigate<GeneralAnalysisViewModel, List<CombatModel>>(loadedCombats.ToList());

            _handler.PropertyUpdate<BasicTemplateViewModel>(Templates.Basic, "Combats", loadedCombats.ToList());
        }

        private async Task DeleteAsync()
        {
            await _combatParserAPIService.DeleteCombatLogAsync(CombatLogs[SelectedCombatLogId].Id);
            await LoadCombatLogsAsync();

            IsParsing = false;
        }
    }
}
