using AutoMapper;
using CombatAnalysis.CombatParser.Entities;
using CombatAnalysis.CombatParser.Interfaces;
using CombatAnalysis.Core.Consts;
using CombatAnalysis.Core.Enums;
using CombatAnalysis.Core.Interfaces;
using CombatAnalysis.Core.Interfaces.Observers;
using CombatAnalysis.Core.Localizations;
using CombatAnalysis.Core.Models;
using CombatAnalysis.Core.Services;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using System.Collections.ObjectModel;

namespace CombatAnalysis.Core.ViewModels;

public class CombatLogInformationViewModel : MvxViewModel, IObserver, IAuthObserver
{
    private readonly IMvxNavigationService _mvvmNavigation;
    private readonly IMapper _mapper;
    private readonly IParser _parser;
    private readonly CombatParserAPIService _combatParserAPIService;

    private string _combatLog;
    private bool _fileIsNotCorrect;
    private bool _isParsing;
    private bool _isNeedSave;
    private bool _isShowSteps;
    private string _foundCombat;
    private string _combatLogPath;
    private int _combatListSelectedIndex;
    private int _selectedCombatLogTypeTabItem;
    private IImprovedMvxViewModel _basicTemplate;
    private ObservableCollection<CombatLogModel> _combatLogs = new ObservableCollection<CombatLogModel>();
    private ObservableCollection<CombatLogModel> _combatLogsByUser = new ObservableCollection<CombatLogModel>();
    private double _screenWidth;
    private double _screenHeight;
    private bool _isAuth;
    private bool _isAllowSaveLogs = true;
    private LogType _logType;
    private LoadingStatus _combatLogLoadingStatus;
    private LoadingStatus _combatLogByUserLoadingStatus;

    public CombatLogInformationViewModel(IMapper mapper, IMvxNavigationService mvvmNavigation, IHttpClientHelper httpClient, IParser parser, ILogger logger, IMemoryCache memoryCache)
    {
        _mapper = mapper;
        _mvvmNavigation = mvvmNavigation;
        _parser = parser;

        OpenPlayerAnalysisCommand = new MvxAsyncCommand(OpenPlayerAnalysisAsync);
        LoadCombatsCommand = new MvxAsyncCommand(() => LoadCombatsAsync(CombatLogs));
        LoadCombatsByUserCommand = new MvxAsyncCommand(() => LoadCombatsAsync(CombatLogsByUser));
        ReloadCombatsCommand = new MvxAsyncCommand(LoadCombatLogsAsync);
        ReloadCombatsByUserCommand = new MvxAsyncCommand(LoadCombatLogsByUserAsync);
        DeleteCombatCommand = new MvxAsyncCommand(DeleteAsync);

        GetLogTypeCommand = new MvxCommand<int>(GetLogType);

        _combatParserAPIService = new CombatParserAPIService(httpClient, logger, memoryCache);

        BasicTemplate = Templates.Basic;
        BasicTemplate.Parent = this;
        BasicTemplate.Handler.PropertyUpdate<BasicTemplateViewModel>(BasicTemplate, nameof(BasicTemplateViewModel.Step), 0);
        BasicTemplate.Handler.PropertyUpdate<BasicTemplateViewModel>(BasicTemplate, nameof(BasicTemplateViewModel.LogPanelStatusIsVisibly), true);

        var authObservable = (IAuthObservable)BasicTemplate;
        authObservable.AddObserver(this);
    }

    #region Commands

    public IMvxCommand GetCombatLogCommand { get; set; }

    public IMvxAsyncCommand LoadCombatsCommand { get; set; }

    public IMvxAsyncCommand LoadCombatsByUserCommand { get; set; }

    public IMvxAsyncCommand ReloadCombatsCommand { get; set; }

    public IMvxAsyncCommand ReloadCombatsByUserCommand { get; set; }

    public IMvxAsyncCommand DeleteCombatCommand { get; set; }

    public IMvxAsyncCommand OpenPlayerAnalysisCommand { get; set; }

    public IMvxCommand<int> GetLogTypeCommand { get; set; }

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

    public string CombatLogPath
    {
        get { return _combatLogPath; }
        set
        {
            SetProperty(ref _combatLogPath, value);
            GetCombatLog();
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

    public int CombatListSelectedIndex
    {
        get { return _combatListSelectedIndex; }
        set
        {
            SetProperty(ref _combatListSelectedIndex, value);
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

    public bool IsAllowSaveLogs
    {
        get { return _isAllowSaveLogs; }
        set
        {
            SetProperty(ref _isAllowSaveLogs, value);
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
            BasicTemplate.Handler.PropertyUpdate<BasicTemplateViewModel>(BasicTemplate, nameof(BasicTemplateViewModel.LogType), value);
        }
    }

    public LoadingStatus CombatLogLoadingStatus
    {
        get { return _combatLogLoadingStatus; }
        set
        {
            SetProperty(ref _combatLogLoadingStatus, value);
        }
    }

    public LoadingStatus CombatLogByUserLoadingStatus
    {
        get { return _combatLogByUserLoadingStatus; }
        set
        {
            SetProperty(ref _combatLogByUserLoadingStatus, value);
        }
    }

    #endregion

    public void GetCombatLog()
    {
        var split = CombatLogPath.Split(@"\");
        CombatLog = split[split.Length - 1];
    }

    public async Task OpenPlayerAnalysisAsync()
    {
        await CombatLogFileValidateAsync(_combatLogPath);
    }

    public void Update(Combat data)
    {
        var dungeon = TranslationSource.Instance["CombatAnalysis.App.Localizations.Resources.CombatLogInformation.Resource.Dungeon"];
        var combat = TranslationSource.Instance["CombatAnalysis.App.Localizations.Resources.CombatLogInformation.Resource.Combat"];
        var time = TranslationSource.Instance["CombatAnalysis.App.Localizations.Resources.CombatLogInformation.Resource.Time"];
        var result = TranslationSource.Instance["CombatAnalysis.App.Localizations.Resources.CombatLogInformation.Resource.Result"];

        FoundCombat = $"{dungeon}: {data.DungeonName}, {combat}: {data.Name}, {time}: {data.Duration}, {result}: {data.IsWin}";
    }

    public void GetLogType(int logType)
    {
        LogType = (LogType)logType;
    }

    public override void ViewAppeared()
    {
        IsParsing = false;

        CombatLogs?.Clear();

        Task.Run(LoadCombatLogsAsync);
        Task.Run(LoadCombatLogsByUserAsync);
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

    public async Task LoadCombatsAsync(ObservableCollection<CombatLogModel> combatCollection)
    {
        var id = combatCollection[CombatListSelectedIndex].Id;
        var loadedCombats = await _combatParserAPIService.LoadCombatsAsync(id);

        foreach (var item in loadedCombats)
        {
            var players = await _combatParserAPIService.LoadCombatPlayersAsync(item.Id);
            item.Players = players.ToList();
        }

        BasicTemplate.Handler.PropertyUpdate<BasicTemplateViewModel>(BasicTemplate, nameof(BasicTemplateViewModel.AllowStep), 1);

        var dataForGeneralAnalysis = Tuple.Create(loadedCombats.ToList(), LogType);
        await _mvvmNavigation.Navigate<GeneralAnalysisViewModel, Tuple<List<CombatModel>, LogType>>(dataForGeneralAnalysis);

        BasicTemplate.Handler.PropertyUpdate<GeneralAnalysisViewModel>(BasicTemplate.Parent, nameof(GeneralAnalysisViewModel.CombatLogId), id);
        BasicTemplate.Handler.PropertyUpdate<BasicTemplateViewModel>(BasicTemplate, nameof(BasicTemplateViewModel.Combats), loadedCombats.ToList());
    }

    public async Task DeleteAsync()
    {
        FoundCombat = string.Empty;
        IsParsing = true;

        await _combatParserAPIService.DeleteCombatLogAsync(CombatLogsByUser[CombatListSelectedIndex].Id);
        await LoadCombatLogsByUserAsync();

        IsParsing = false;
    }

    private async Task CombatLogFileValidateAsync(string combatLog)
    {
        _parser.AddObserver(this);
        FileIsNotCorrect = !await _parser.FileCheck(combatLog);

        if (!FileIsNotCorrect)
        {
            await PrepareCombatData(combatLog);
        }
    }

    private async Task PrepareCombatData(string combatLog)
    {
        IsParsing = true;

        await _parser.Parse(combatLog);

        var combatModels = _mapper.Map<List<CombatModel>>(_parser.Combats);

        BasicTemplate.Handler.PropertyUpdate<BasicTemplateViewModel>(BasicTemplate, nameof(BasicTemplateViewModel.AllowStep), 1);

        var dataForGeneralAnalysis = Tuple.Create(combatModels, LogType);
        await _mvvmNavigation.Navigate<GeneralAnalysisViewModel, Tuple<List<CombatModel>, LogType>>(dataForGeneralAnalysis);

        BasicTemplate.Handler.PropertyUpdate<BasicTemplateViewModel>(BasicTemplate, nameof(BasicTemplateViewModel.Combats), combatModels);

        if (IsNeedSave)
        {
            BasicTemplate.Handler.PropertyUpdate<BasicTemplateViewModel>(BasicTemplate, nameof(BasicTemplateViewModel.ResponseStatus), LoadingStatus.Pending);

            var combatLogId = await _combatParserAPIService.SaveAsync(combatModels, LogType);
            BasicTemplate.Handler.PropertyUpdate<GeneralAnalysisViewModel>(BasicTemplate.SavedViewModel, nameof(GeneralAnalysisViewModel.CombatLogId), combatLogId);

            var responseStatus = combatLogId > 0 ? LoadingStatus.Successful : LoadingStatus.Failed;
            BasicTemplate.Handler.PropertyUpdate<BasicTemplateViewModel>(BasicTemplate, nameof(BasicTemplateViewModel.ResponseStatus), responseStatus);
        }
    }

    private async Task LoadCombatLogsAsync()
    {
        CombatLogLoadingStatus = LoadingStatus.Pending;

        _combatParserAPIService.SetUpPort();

        var combatLogsData = await _combatParserAPIService.LoadCombatLogsAsync();
        if (combatLogsData == null)
        {
            CombatLogLoadingStatus = LoadingStatus.Failed;
            return;
        }

        var readyCombatLogData = new List<CombatLogModel>();

        foreach (var item in combatLogsData)
        {
            if (item.IsReady)
            {
                readyCombatLogData.Add(item);
            }
        }

        CombatLogs = new ObservableCollection<CombatLogModel>(readyCombatLogData);

        CombatLogLoadingStatus = LoadingStatus.Successful;
    }

    private async Task LoadCombatLogsByUserAsync()
    {
        CombatLogByUserLoadingStatus = LoadingStatus.Pending;

        _combatParserAPIService.SetUpPort();

        var combatLogsData = await _combatParserAPIService.LoadCombatLogsByUserAsync();
        if (combatLogsData == null)
        {
            CombatLogByUserLoadingStatus = LoadingStatus.Failed;
            return;
        }

        var readyCombatLogData = new List<CombatLogModel>();

        foreach (var item in combatLogsData)
        {
            if (item.IsReady)
            {
                readyCombatLogData.Add(item);
            }
        }

        CombatLogsByUser = new ObservableCollection<CombatLogModel>(readyCombatLogData);

        CombatLogByUserLoadingStatus = LoadingStatus.Successful;
    }
}
