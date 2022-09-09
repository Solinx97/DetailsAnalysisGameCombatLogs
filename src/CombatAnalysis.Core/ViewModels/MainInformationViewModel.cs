using AutoMapper;
using CombatAnalysis.CombatParser.Interfaces;
using CombatAnalysis.Core.Commands;
using CombatAnalysis.Core.Consts;
using CombatAnalysis.Core.Core;
using CombatAnalysis.Core.Enums;
using CombatAnalysis.Core.Interfaces;
using CombatAnalysis.Core.Interfaces.Observers;
using CombatAnalysis.Core.Models;
using CombatAnalysis.Core.Services;
using CombatAnalysis.WinCore;
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
    public class MainInformationViewModel : MvxViewModel, IObserver, IAuthObserver
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
        private int _selectedCombatLogTypeTabItem;
        private int _combatLogsNumber;
        private int _combatLogsByUserNumber;
        private IImprovedMvxViewModel _basicTemplate;
        private IViewModelConnect _handler;
        private ObservableCollection<CombatLogModel> _combatLogs;
        private ObservableCollection<CombatLogModel> _combatLogsByUser;
        private double _screenWidth;
        private double _screenHeight;
        private bool _isAuth;
        private LogType _logType;
        private ObservableCollection<CombatLogModel>[] _combatLogLists = new ObservableCollection<CombatLogModel>[2];

        public MainInformationViewModel(IMapper mapper, IMvxNavigationService mvvmNavigation, IHttpClientHelper httpClient, IParser parser, ILogger logger, IMemoryCache memoryCache)
        {
            _mapper = mapper;
            _mvvmNavigation = mvvmNavigation;
            _parser = parser;

            GetCombatLogCommand = new MvxCommand(GetCombatLog);
            OpenPlayerAnalysisCommand = new MvxCommand(OpenPlayerAnalysis);
            LoadCombatsCommand = new MvxCommand(LoadCombats);
            ReloadCombatsCommand = new MvxCommand(LoadCombatLogs);
            DeleteCombatCommand = new MvxCommand(DeleteCombat);

            GetLogTypeCommand = new MvxCommand<int>(GetLogType);

            _combatParserAPIService = new CombatParserAPIService(httpClient, logger, memoryCache);
            _handler = new ViewModelMConnect();

            BasicTemplate = new BasicTemplateViewModel(_handler, mvvmNavigation, memoryCache, httpClient);
            Templates.Basic = BasicTemplate;

            var authObservable = (IAuthObservable)BasicTemplate;
            authObservable.AddObserver(this);

            ((BasicTemplateViewModel)BasicTemplate).CheckAuth();
        }

        public IMvxCommand GetCombatLogCommand { get; set; }

        public IMvxCommand LoadCombatsCommand { get; set; }

        public IMvxCommand ReloadCombatsCommand { get; set; }

        public IMvxCommand DeleteCombatCommand { get; set; }

        public IMvxCommand OpenPlayerAnalysisCommand { get; set; }

        public IMvxCommand<int> GetLogTypeCommand { get; set; }

        public IImprovedMvxViewModel BasicTemplate
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

        public ObservableCollection<CombatLogModel> CombatLogsByUser
        {
            get { return _combatLogsByUser; }
            set
            {
                SetProperty(ref _combatLogsByUser, value);
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

        public int SelectedCombatLogTypeTabItem
        {
            get { return _selectedCombatLogTypeTabItem; }
            set
            {
                SetProperty(ref _selectedCombatLogTypeTabItem, value);
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

        public int CombatLogsByUserNumber
        {
            get { return _combatLogsByUserNumber; }
            set
            {
                SetProperty(ref _combatLogsByUserNumber, value);
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

        public bool IsAuth
        {
            get { return _isAuth; }
            set
            {
                SetProperty(ref _isAuth, value);
            }
        }

        public LogType LogType
        {
            get { return _logType; }
            set
            {
                SetProperty(ref _logType, value);
                BasicTemplate.Handler.PropertyUpdate<BasicTemplateViewModel>(BasicTemplate, "LogType", value);
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
            Task.Run(() => LoadCombatLogsByUserAsync());
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
            Task.Run(() => CombatLogFileValidate(_combatLogPath));
        }

        public void Update(string combatInformation)
        {
            FoundCombat = combatInformation;
        }

        public void GetLogType(int logType)
        {
            LogType = (LogType)logType;
        }

        public override void ViewAppeared()
        {
            IsParsing = false;

            CombatLogs?.Clear();
            LoadCombatLogs();

            ScreenWidth = System.Windows.SystemParameters.PrimaryScreenWidth * 0.75;
            ScreenHeight = System.Windows.SystemParameters.PrimaryScreenHeight * 0.75;
        }

        public void AuthUpdate(bool isAuth)
        {
            IsAuth = isAuth;
            if (!isAuth)
            {
                LogType = LogType.NotIncludePlayer;
                SelectedCombatLogTypeTabItem = 0;
            }
        }

        private async Task CombatLogFileValidate(string combatLog)
        {
            _parser.AddObserver(this);
            FileIsNotCorrect = !await _parser.FileCheck(combatLog);

            if (!FileIsNotCorrect)
            {
                await GetCombatDataDetails(combatLog);
            }
        }

        private async Task GetCombatDataDetails(string combatLog)
        {
            IsParsing = true;

            await _parser.Parse(combatLog);

            var map = _parser.Combats;
            var combats = _mapper.Map<List<CombatModel>>(map);

            BasicTemplate.Handler.PropertyUpdate<BasicTemplateViewModel>(Templates.Basic, "AllowStep", 1);

            var dataForGeneralAnalysis = Tuple.Create(combats, LogType);
            await _mvvmNavigation.Navigate<GeneralAnalysisViewModel, Tuple<List<CombatModel>, LogType>>(dataForGeneralAnalysis);

            BasicTemplate.Handler.PropertyUpdate<BasicTemplateViewModel>(BasicTemplate, "Combats", combats);

            if (IsNeedSave)
            {
                BasicTemplate.Handler.PropertyUpdate<BasicTemplateViewModel>(BasicTemplate, "ResponseStatus", ResponseStatus.Pending);

                var responseStatus = await _combatParserAPIService.Save(combats, LogType).ConfigureAwait(false) ? ResponseStatus.Successful : ResponseStatus.Failed;

                BasicTemplate.Handler.PropertyUpdate<BasicTemplateViewModel>(BasicTemplate, "ResponseStatus", responseStatus);
            }
        }

        private async Task LoadCombatLogsAsync()
        {
            _combatParserAPIService.SetUpPort();

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

            _combatLogLists[0] = CombatLogs;
        }

        private async Task LoadCombatLogsByUserAsync()
        {
            _combatParserAPIService.SetUpPort();

            var combatLogsData = await _combatParserAPIService.LoadCombatLogsByUserAsync();
            var readyCombatLogData = new List<CombatLogModel>();

            foreach (var item in combatLogsData)
            {
                if (item.IsReady)
                {
                    readyCombatLogData.Add(item);
                }
            }

            CombatLogsByUser = new ObservableCollection<CombatLogModel>(readyCombatLogData);
            CombatLogsByUserNumber = CombatLogsByUser.Count;

            _combatLogLists[1] = CombatLogsByUser;
        }

        private async Task LoadCombatsAsync()
        {
            var id = _combatLogLists[SelectedCombatLogTypeTabItem][SelectedCombatLogId].Id;
            var loadedCombats = await _combatParserAPIService.LoadCombatsAsync(id);

            foreach (var item in loadedCombats)
            {
                var players = await _combatParserAPIService.LoadCombatPlayersAsync(item.Id);
                item.Players = players.ToList();
            }

            BasicTemplate.Handler.PropertyUpdate<BasicTemplateViewModel>(BasicTemplate, "AllowStep", 1);

            var dataForGeneralAnalysis = Tuple.Create(loadedCombats.ToList(), LogType);
            await _mvvmNavigation.Navigate<GeneralAnalysisViewModel, Tuple<List<CombatModel>, LogType>>(dataForGeneralAnalysis);

            BasicTemplate.Handler.PropertyUpdate<BasicTemplateViewModel>(BasicTemplate, "Combats", loadedCombats.ToList());
        }

        private async Task DeleteAsync()
        {
            await _combatParserAPIService.DeleteCombatLogAsync(CombatLogs[SelectedCombatLogId].Id);
            await LoadCombatLogsAsync();
            await LoadCombatLogsByUserAsync();

            IsParsing = false;
        }
    }
}
