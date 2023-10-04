using AutoMapper;
using CombatAnalysis.CombatParser.Entities;
using CombatAnalysis.CombatParser.Interfaces;
using CombatAnalysis.Core.Enums;
using CombatAnalysis.Core.Interfaces;
using CombatAnalysis.Core.Interfaces.Observers;
using CombatAnalysis.Core.Localizations;
using CombatAnalysis.Core.Models;
using CombatAnalysis.Core.Models.User;
using CombatAnalysis.Core.Modelsl;
using CombatAnalysis.Core.Services;
using CombatAnalysis.Core.ViewModels.Base;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using System.Collections.ObjectModel;

namespace CombatAnalysis.Core.ViewModels;

public class CombatLogInformationViewModel : ParentTemplate, IObserver, IAuthObserver
{
    private readonly IMvxNavigationService _mvvmNavigation;
    private readonly IMapper _mapper;
    private readonly IParser _parser;
    private readonly CombatParserAPIService _combatParserAPIService;
    private readonly IMemoryCache _memoryCache;

    private string _combatLog;
    private bool _fileIsNotCorrect;
    private bool _isParsing;
    private bool _combatLogUploadingFailed;
    private bool _isNeedSave = true;
    private bool _isShowSteps;
    private string _dungeonName;
    private string _combatName;
    private string _combatStatus;
    private string _combatLogPath;
    private int _combatListSelectedIndex;
    private int _selectedCombatLogTypeTabItem;
    private ObservableCollection<CombatLogModel> _combatLogs = new ObservableCollection<CombatLogModel>();
    private ObservableCollection<CombatLogModel> _combatLogsForTargetUser = new ObservableCollection<CombatLogModel>();
    private ObservableCollection<CombatLogByUserModel> _combatLogsByUser = new ObservableCollection<CombatLogByUserModel>();
    private double _screenWidth;
    private double _screenHeight;
    private bool _isAuth;
    private bool _isAllowSaveLogs = true;
    private LogType _logType;
    private LoadingStatus _combatLogLoadingStatus;
    private LoadingStatus _combatLogByUserLoadingStatus;
    private bool _removingInProgress;

    public CombatLogInformationViewModel(IMapper mapper, IMvxNavigationService mvvmNavigation, IHttpClientHelper httpClient, 
        IParser parser, ILogger logger, IMemoryCache memoryCache)
    {
        _mapper = mapper;
        _mvvmNavigation = mvvmNavigation;
        _parser = parser;
        _memoryCache = memoryCache;

        OpenPlayerAnalysisCommand = new MvxAsyncCommand(OpenPlayerAnalysisAsync);
        LoadCombatsCommand = new MvxAsyncCommand(() => LoadCombatsAsync(CombatLogs));
        LoadCombatsByUserCommand = new MvxAsyncCommand(() => LoadCombatsAsync(CombatLogsForTargetUser));
        ReloadCombatsCommand = new MvxAsyncCommand(LoadCombatLogsAsync);
        ReloadCombatsByUserCommand = new MvxAsyncCommand(LoadCombatLogsByUserAsync);
        DeleteCombatCommand = new MvxAsyncCommand(DeleteAsync);

        GetLogTypeCommand = new MvxCommand<int>(GetLogType);

        _combatParserAPIService = new CombatParserAPIService(httpClient, logger, memoryCache);

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

    public ObservableCollection<CombatLogModel> CombatLogs
    {
        get { return _combatLogs; }
        set
        {
            SetProperty(ref _combatLogs, value);
        }
    }

    public ObservableCollection<CombatLogModel> CombatLogsForTargetUser
    {
        get { return _combatLogsForTargetUser; }
        set
        {
            SetProperty(ref _combatLogsForTargetUser, value);
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

    public bool CombatLogUploadingFailed
    {
        get { return _combatLogUploadingFailed; }
        set
        {
            SetProperty(ref _combatLogUploadingFailed, value);
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

    public string DungeonName
    {
        get { return _dungeonName; }
        set
        {
            SetProperty(ref _dungeonName, value);
        }
    }

    public string CombatName
    {
        get { return _combatName; }
        set
        {
            SetProperty(ref _combatName, value);
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

    public bool RemovingInProgress
    {
        get { return _removingInProgress; }
        set
        {
            SetProperty(ref _removingInProgress, value);
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
        CombatLogUploadingFailed = false;
        BasicTemplate.Handler.PropertyUpdate<BasicTemplateViewModel>(BasicTemplate, nameof(BasicTemplateViewModel.ResponseStatus), LoadingStatus.None);

        await CombatLogFileValidateAsync(_combatLogPath);
    }

    public void Update(Combat data)
    {
        var dungeon = TranslationSource.Instance["CombatAnalysis.App.Localizations.Resources.CombatLogInformation.Resource.Dungeon"];
        var combat = TranslationSource.Instance["CombatAnalysis.App.Localizations.Resources.CombatLogInformation.Resource.Combat"];
        var time = TranslationSource.Instance["CombatAnalysis.App.Localizations.Resources.CombatLogInformation.Resource.Time"];
        var result = TranslationSource.Instance["CombatAnalysis.App.Localizations.Resources.CombatLogInformation.Resource.Result"];
        var win = TranslationSource.Instance["CombatAnalysis.App.Localizations.Resources.CombatLogInformation.Resource.Win"];
        var lose = TranslationSource.Instance["CombatAnalysis.App.Localizations.Resources.CombatLogInformation.Resource.Lose"];

        DungeonName = data.DungeonName;
        CombatName = data.Name;
        CombatStatus = data.IsWin ? win : lose;
    }

    public void GetLogType(int logType)
    {
        LogType = (LogType)logType;
    }

    public override void ViewAppeared()
    {
        IsParsing = false;

        CombatLogs?.Clear();
        CheckAuth();

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
        var combatLog = combatCollection[CombatListSelectedIndex];
        var loadedCombats = await _combatParserAPIService.LoadCombatsAsync(combatLog.Id);
        if (loadedCombats == null)
        {
            return;
        }

        foreach (var item in loadedCombats)
        {
            var players = await _combatParserAPIService.LoadCombatPlayersAsync(item.Id);
            item.Players = players.ToList();
        }

        BasicTemplate.Handler.PropertyUpdate<BasicTemplateViewModel>(BasicTemplate, nameof(BasicTemplateViewModel.AllowStep), 1);

        var dataForGeneralAnalysis = Tuple.Create(loadedCombats.ToList(), LogType);
        await _mvvmNavigation.Navigate<GeneralAnalysisViewModel, Tuple<List<CombatModel>, LogType>>(dataForGeneralAnalysis);

        BasicTemplate.Handler.PropertyUpdate<GeneralAnalysisViewModel>(BasicTemplate.Parent, nameof(GeneralAnalysisViewModel.CombatLog), combatLog);
        BasicTemplate.Handler.PropertyUpdate<BasicTemplateViewModel>(BasicTemplate, nameof(BasicTemplateViewModel.Combats), loadedCombats.ToList());
    }

    public async Task DeleteAsync()
    {
        DungeonName = string.Empty;
        CombatName = string.Empty;
        CombatStatus = string.Empty;
        RemovingInProgress = true;

        var selectedCombatLogByUser = _combatLogsByUser.FirstOrDefault(x => x.CombatLogId == CombatLogsForTargetUser[CombatListSelectedIndex].Id);
        await _combatParserAPIService.DeleteCombatLogByUserAsync(selectedCombatLogByUser.Id);
        await LoadCombatLogsByUserAsync();

        RemovingInProgress = false;
    }

    private void CheckAuth()
    {
        var user = _memoryCache.Get<AppUserModel>(nameof(MemoryCacheValue.User));
        IsAuth = user != null;
    }

    private async Task CombatLogFileValidateAsync(string combatLog)
    {
        _parser.AddObserver(this);
        FileIsNotCorrect = !await _parser.FileCheck(combatLog);

        if (!FileIsNotCorrect)
        {
            IsParsing = true;

            await PrepareCombatData(combatLog);

            IsParsing = false;
        }
    }

    private async Task PrepareCombatData(string combatLogData)
    {
        await _parser.Parse(combatLogData);

        var combatModels = _mapper.Map<List<CombatModel>>(_parser.Combats);

        var dataForGeneralAnalysis = Tuple.Create(combatModels, LogType);
        if (!IsNeedSave)
        {
            BasicTemplate.Handler.PropertyUpdate<BasicTemplateViewModel>(BasicTemplate, nameof(BasicTemplateViewModel.AllowStep), 1);
            BasicTemplate.Handler.PropertyUpdate<BasicTemplateViewModel>(BasicTemplate, nameof(BasicTemplateViewModel.ResponseStatus), LoadingStatus.None);

            await _mvvmNavigation.Navigate<GeneralAnalysisViewModel, Tuple<List<CombatModel>, LogType>>(dataForGeneralAnalysis);

            BasicTemplate.Handler.PropertyUpdate<BasicTemplateViewModel>(BasicTemplate, nameof(BasicTemplateViewModel.Combats), combatModels);
            return;
        }

        await UploadingCombatLogAsync(combatModels, dataForGeneralAnalysis);
    }

    private async Task UploadingCombatLogAsync(List<CombatModel> combatList, Tuple<List<CombatModel>, LogType> dataForGeneralAnalysis)
    {
        BasicTemplate.Handler.PropertyUpdate<BasicTemplateViewModel>(BasicTemplate, nameof(BasicTemplateViewModel.ResponseStatus), LoadingStatus.Pending);

        var createdCombatLog = await _combatParserAPIService.SaveCombatLogAsync(combatList);
        if (createdCombatLog == null)
        {
            BasicTemplate.Handler.PropertyUpdate<BasicTemplateViewModel>(BasicTemplate, nameof(BasicTemplateViewModel.ResponseStatus), LoadingStatus.Failed);

            CombatLogUploadingFailed = true;
            return;
        }

        BasicTemplate.Handler.PropertyUpdate<BasicTemplateViewModel>(BasicTemplate, nameof(BasicTemplateViewModel.AllowStep), 1);

        await _mvvmNavigation.Navigate<GeneralAnalysisViewModel, Tuple<List<CombatModel>, LogType>>(dataForGeneralAnalysis);

        BasicTemplate.Handler.PropertyUpdate<GeneralAnalysisViewModel>(BasicTemplate.SavedViewModel, nameof(GeneralAnalysisViewModel.ResponseStatus), LoadingStatus.Pending);

        BasicTemplate.Handler.PropertyUpdate<BasicTemplateViewModel>(BasicTemplate, nameof(BasicTemplateViewModel.Combats), combatList);
        BasicTemplate.Handler.PropertyUpdate<GeneralAnalysisViewModel>(BasicTemplate.SavedViewModel, nameof(GeneralAnalysisViewModel.CombatLog), createdCombatLog);

        var combatsAreUploaded = await _combatParserAPIService.SaveAsync(combatList, createdCombatLog, LogType, CombatUploaded);
        if (combatsAreUploaded == null)
        {
            BasicTemplate.Handler.PropertyUpdate<BasicTemplateViewModel>(BasicTemplate, nameof(BasicTemplateViewModel.ResponseStatus), LoadingStatus.Failed);
            return;
        }

        var responseStatus = combatsAreUploaded.Any(uploaded => !uploaded) ? LoadingStatus.Failed : LoadingStatus.Successful;
        BasicTemplate.Handler.PropertyUpdate<BasicTemplateViewModel>(BasicTemplate, nameof(BasicTemplateViewModel.ResponseStatus), responseStatus);
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

        var combatLogsByUser = await _combatParserAPIService.LoadCombatLogsByUserAsync();
        if (combatLogsByUser == null)
        {
            CombatLogByUserLoadingStatus = LoadingStatus.Failed;
            return;
        }

        var combatLogsIdByUser = combatLogsByUser.GroupBy(x => x.CombatLogId).Select(x => x.Key);
        var combatLogs = await _combatParserAPIService.LoadCombatLogsAsync(combatLogsIdByUser.ToList());
        if (combatLogs == null)
        {
            CombatLogLoadingStatus = LoadingStatus.Failed;
            return;
        }

        var readyCombatLogData = new List<CombatLogModel>();
        foreach (var item in combatLogs)
        {
            if (item.IsReady)
            {
                readyCombatLogData.Add(item);
            }
        }

        CombatLogsForTargetUser = new ObservableCollection<CombatLogModel>(readyCombatLogData);
        _combatLogsByUser = new ObservableCollection<CombatLogByUserModel>(combatLogsByUser);

        CombatLogByUserLoadingStatus = LoadingStatus.Successful;
    }

    private void CombatUploaded(int number, string dungeonName, string name)
    {
        BasicTemplate.Handler.PropertyUpdate<GeneralAnalysisViewModel>(BasicTemplate.SavedViewModel, nameof(GeneralAnalysisViewModel.CurrentCombatNumber), number);
        BasicTemplate.Handler.PropertyUpdate<GeneralAnalysisViewModel>(BasicTemplate.SavedViewModel, nameof(GeneralAnalysisViewModel.DungeonName), dungeonName);
        BasicTemplate.Handler.PropertyUpdate<GeneralAnalysisViewModel>(BasicTemplate.SavedViewModel, nameof(GeneralAnalysisViewModel.Name), name);
    }
}
