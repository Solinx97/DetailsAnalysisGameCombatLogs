using AutoMapper;
using CombatAnalysis.CombatParser.Details;
using CombatAnalysis.CombatParser.Entities;
using CombatAnalysis.CombatParser.Services;
using CombatAnalysis.Core.Consts;
using CombatAnalysis.Core.Enums;
using CombatAnalysis.Core.Interfaces;
using CombatAnalysis.Core.Interfaces.Observers;
using CombatAnalysis.Core.Models;
using CombatAnalysis.Core.Models.User;
using CombatAnalysis.Core.Services;
using CombatAnalysis.Core.ViewModels.Base;
using CombatAnalysis.Core.ViewModels.ViewModelTemplates;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using System.Collections.ObjectModel;

namespace CombatAnalysis.Core.ViewModels;

public class CombatLogInformationViewModel : ParentTemplate, CombatParser.Interfaces.IObserver<Combat>, IAuthObserver
{
    private readonly IMvxNavigationService _mvvmNavigation;
    private readonly IMapper _mapper;
    private readonly ICacheService _cacheService;
    private readonly CombatParserService _parser;
    private readonly CombatParserAPIService _combatParserAPIService;
    private readonly IMemoryCache _memoryCache;

    private string? _combatLog;
    private string? _dungeonName;
    private string? _combatName;
    private string? _combatLogPath;
    private bool _isNeedSave;
    private ObservableCollection<CombatLogModel> _combatLogs = new();
    private ObservableCollection<CombatLogModel> _combatLogsForTargetUser = new();
    private bool _isAllowSaveLogs = true;
    private CancellationTokenSource _cancellationTokenSource = new();

    private bool _fileIsCorrect = true;
    private bool _openUploadedLogs;
    private bool _isParsing;
    private bool _combatLogUploadingFailed;
    private int _combatListSelectedIndex;
    private int _selectedCombatLogTypeTabItem;
    private bool _isAuth;
    private LogType _logType;
    private LoadingStatus _combatLogLoadingStatus;
    private LoadingStatus _combatLogByUserLoadingStatus;
    private bool _removingInProgress;
    private bool _uploadingLogs;
    private bool _noCombatsUploaded;
    private bool _processAborted;

    public CombatLogInformationViewModel(IMapper mapper, IMvxNavigationService mvvmNavigation, IHttpClientHelper httpClient,
        CombatParserService parser, ILogger logger, IMemoryCache memoryCache, ICacheService cacheService)
    {
        _mapper = mapper;
        _mvvmNavigation = mvvmNavigation;
        _parser = parser;
        _memoryCache = memoryCache;
        _cacheService = cacheService;

        OpenUploadedLogsCommand = new MvxCommand(() => OpenUploadedLogs = !OpenUploadedLogs);
        OpenPlayerAnalysisCommand = new MvxAsyncCommand(OpenPlayerAnalysisAsync);
        LoadCombatsCommand = new MvxAsyncCommand(() => LoadCombatsAsync(CombatLogs));
        LoadCombatsByUserCommand = new MvxAsyncCommand(() => LoadCombatsAsync(CombatLogsForTargetUser));
        ReloadCombatsCommand = new MvxAsyncCommand(LoadCombatLogsAsync);
        DeleteCombatCommand = new MvxAsyncCommand(DeleteAsync);
        CancelParsingCommand = new MvxCommand(CancelParsing);

        GetLogTypeCommand = new MvxCommand<int>(GetLogType);

        _combatParserAPIService = new CombatParserAPIService(httpClient, logger, memoryCache);

        Basic.Parent = this;
        Basic.Handler.BasicPropertyUpdate(nameof(BasicTemplateViewModel.Step), 0);
        Basic.Handler.BasicPropertyUpdate(nameof(BasicTemplateViewModel.LogPanelStatusIsVisibly), true);

        var authObservable = Basic as IAuthObservable;
        authObservable?.AddObserver(this);

        IsAuth = false;
    }

    #region Commands

    public IMvxCommand OpenUploadedLogsCommand { get; set; }

    public IMvxAsyncCommand LoadCombatsCommand { get; set; }

    public IMvxAsyncCommand LoadCombatsByUserCommand { get; set; }

    public IMvxAsyncCommand ReloadCombatsCommand { get; set; }

    public IMvxAsyncCommand DeleteCombatCommand { get; set; }

    public IMvxAsyncCommand OpenPlayerAnalysisCommand { get; set; }

    public IMvxCommand<int> GetLogTypeCommand { get; set; }

    public IMvxCommand CancelParsingCommand { get; set; }

    #endregion

    #region View model properties

    public bool OpenUploadedLogs
    {
        get { return _openUploadedLogs; }
        set
        {
            SetProperty(ref _openUploadedLogs, value);
        }
    }

    public bool NoCombatsUploaded
    {
        get { return _noCombatsUploaded; }
        set
        {
            SetProperty(ref _noCombatsUploaded, value);
        }
    }

    public bool UploadingLogs
    {
        get { return _uploadingLogs; }
        set
        {
            SetProperty(ref _uploadingLogs, value);
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

    public ObservableCollection<CombatLogModel> CombatLogsForTargetUser
    {
        get { return _combatLogsForTargetUser; }
        set
        {
            SetProperty(ref _combatLogsForTargetUser, value);
        }
    }

    public string? CombatLog
    {
        get { return _combatLog; }
        set
        {
            SetProperty(ref _combatLog, value);
        }
    }

    public string? CombatLogPath
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
            if (value)
            {
                OpenUploadedLogs = false;
            }
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

    public bool FileIsCorrect
    {
        get { return _fileIsCorrect; }
        set
        {
            SetProperty(ref _fileIsCorrect, value);
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

    public string? DungeonName
    {
        get { return _dungeonName; }
        set
        {
            SetProperty(ref _dungeonName, value);
        }
    }

    public string? CombatName
    {
        get { return _combatName; }
        set
        {
            SetProperty(ref _combatName, value);
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
            Basic.Handler.BasicPropertyUpdate(nameof(BasicTemplateViewModel.LogType), value);
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

    public void AuthUpdate(bool isAuth)
    {
        IsAuth = isAuth;
        if (!isAuth)
        {
            LogType = LogType.Public;
            SelectedCombatLogTypeTabItem = 0;
        }
    }

    public void Update(Combat data)
    {
        if (string.IsNullOrEmpty(data.Name))
        {
            return;
        }

        DungeonName = data.DungeonName;
        CombatName = data.Name;
    }

    #region Ovveride methods

    public override void Prepare()
    {
        CombatLogPath = AppStaticData.SelectedCombatLogFilePath;

        base.Prepare();
    }

    public override void ViewAppeared()
    {
        IsParsing = false;

        CombatLogs?.Clear();
        CheckAuth();

        Task.Run(LoadCombatLogsAsync);
    }

    public override void ViewDestroy(bool viewFinishing = true)
    {
        base.ViewDestroy(viewFinishing);
    }

    #endregion

    private void GetLogType(int logType)
    {
        LogType = (LogType)logType;
    }

    private void GetCombatLog()
    {
        if (string.IsNullOrEmpty(CombatLogPath))
        {
            return;
        }

        var split = CombatLogPath.Split(@"\");
        CombatLog = split[^1];
    }

    private async Task OpenPlayerAnalysisAsync()
    {
        CombatLogUploadingFailed = false;
        Basic.Handler.BasicPropertyUpdate(nameof(BasicTemplateViewModel.ResponseStatus), LoadingStatus.None);

        await CombatLogFileValidateAsync(CombatLogPath ?? string.Empty);
    }

    private async Task LoadCombatsAsync(ObservableCollection<CombatLogModel> combatCollection)
    {
        NoCombatsUploaded = false;

        var combatLog = combatCollection[CombatListSelectedIndex];
        if (combatLog.NumberReadyCombats == 0)
        {
            NoCombatsUploaded = true;

            return;
        }

        UploadingLogs = true;

        _combatParserAPIService.SetUpPort();
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

        Basic.Handler.BasicPropertyUpdate(nameof(BasicTemplateViewModel.AllowStep), 1);

        var dataForGeneralAnalysis = Tuple.Create(loadedCombats.ToList(), LogType);
        await _mvvmNavigation.Navigate<CombatsViewModel, Tuple<List<CombatModel>, LogType>>(dataForGeneralAnalysis);

        Basic.Handler.BasicPropertyUpdate(nameof(BasicTemplateViewModel.CombatLog), combatLog);
        Basic.Handler.BasicPropertyUpdate(nameof(BasicTemplateViewModel.Combats), loadedCombats.ToList());
    }

    private async Task DeleteAsync()
    {
        DungeonName = string.Empty;
        CombatName = string.Empty;
        RemovingInProgress = true;

        var selectedCombatLogByUser = _combatLogs.FirstOrDefault(x => x.Id == CombatLogsForTargetUser[CombatListSelectedIndex].Id);
        if (selectedCombatLogByUser != null)
        {
            await _combatParserAPIService.DeleteCombatLogByUserAsync(selectedCombatLogByUser.Id);
        }

        await LoadCombatLogsAsync();

        RemovingInProgress = false;
    }

    private void CancelParsing()
    {
        _processAborted = true;
        _cancellationTokenSource?.Cancel();
    }

    private void CheckAuth()
    {
        var user = _memoryCache.Get<AppUserModel>(nameof(MemoryCacheValue.User));
        IsAuth = user != null;
    }

    private async Task CombatLogFileValidateAsync(string combatLog)
    {
        FileIsCorrect = await _parser.FileCheckAsync(combatLog);

        if (FileIsCorrect)
        {
            IsParsing = true;

            await PrepareCombatDataAsync(combatLog);

            IsParsing = false;
        }
    }

    private async Task PrepareCombatDataAsync(string combatLogData)
    {
        _cancellationTokenSource = new CancellationTokenSource();

        Basic.Handler.BasicPropertyUpdate(nameof(BasicTemplateViewModel.Combats), new List<CombatModel>());
        Basic.Handler.BasicPropertyUpdate(nameof(BasicTemplateViewModel.PetsId), new Dictionary<string, List<string>>());
        Basic.Handler.BasicPropertyUpdate(nameof(BasicTemplateViewModel.AllowStep), 0);

        await _parser.ParseAsync(combatLogData, _cancellationTokenSource.Token);

        ClearCache();

        AppStaticData.PreparedCombatsCount = _parser.Combats.Count;

        SaveDataInCache(_parser.CombatDetails);

        var combatsList = _mapper.Map<List<CombatModel>>(_parser.Combats);

        _parser.Clear();

        var dataForGeneralAnalysis = Tuple.Create(combatsList, LogType);

        if (_processAborted)
        {
            _processAborted = false;

            return;
        }

        if (!IsNeedSave)
        {
            Basic.Handler.BasicPropertyUpdate(nameof(BasicTemplateViewModel.AllowStep), 1);
            Basic.Handler.BasicPropertyUpdate(nameof(BasicTemplateViewModel.ResponseStatus), LoadingStatus.None);

            await _mvvmNavigation.Navigate<CombatsViewModel, Tuple<List<CombatModel>, LogType>>(dataForGeneralAnalysis);
            Basic.Handler.BasicPropertyUpdate(nameof(BasicTemplateViewModel.Combats), combatsList);

            return;
        }

        await UploadingCombatLogAsync(combatsList, dataForGeneralAnalysis);
    }

    private void ClearCache()
    {
        for (var i = 0; i < AppStaticData.PreparedCombatsCount; i++)
        {
            _cacheService.RemoveDataFromCache($"{AppCacheKeys.CombatDetails_DamageDone}_{i}");
            _cacheService.RemoveDataFromCache($"{AppCacheKeys.CombatDetails_DamageDoneGeneral}_{i}");
            _cacheService.RemoveDataFromCache($"{AppCacheKeys.CombatDetails_HealDone}_{i}");
            _cacheService.RemoveDataFromCache($"{AppCacheKeys.CombatDetails_HealDoneGeneral}_{i}");
            _cacheService.RemoveDataFromCache($"{AppCacheKeys.CombatDetails_DamageTaken}_{i}");
            _cacheService.RemoveDataFromCache($"{AppCacheKeys.CombatDetails_DamageTakenGeneral}_{i}");
            _cacheService.RemoveDataFromCache($"{AppCacheKeys.CombatDetails_ResourcesRecovery}_{i}");
            _cacheService.RemoveDataFromCache($"{AppCacheKeys.CombatDetails_ResourcesRecoveryGeneral}_{i}");
        }
    }

    private void SaveDataInCache(List<CombatDetails> combatDetails)
    {
        for (var i = 0; i < combatDetails.Count; i++)
        {
            var currentCombatDetails = combatDetails[i];

            _cacheService.SaveDataToCache($"{AppCacheKeys.CombatDetails_Positions}_{i}", currentCombatDetails.Positions);

            _cacheService.SaveDataToCache($"{AppCacheKeys.CombatDetails_DamageDone}_{i}", currentCombatDetails.DamageDone);
            _cacheService.SaveDataToCache($"{AppCacheKeys.CombatDetails_DamageDoneGeneral}_{i}", currentCombatDetails.DamageDoneGeneral);
            _cacheService.SaveDataToCache($"{AppCacheKeys.CombatDetails_HealDone}_{i}", currentCombatDetails.HealDone);
            _cacheService.SaveDataToCache($"{AppCacheKeys.CombatDetails_HealDoneGeneral}_{i}", currentCombatDetails.HealDoneGeneral);
            _cacheService.SaveDataToCache($"{AppCacheKeys.CombatDetails_DamageTaken}_{i}", currentCombatDetails.DamageTaken);
            _cacheService.SaveDataToCache($"{AppCacheKeys.CombatDetails_DamageTakenGeneral}_{i}", currentCombatDetails.DamageTakenGeneral);
            _cacheService.SaveDataToCache($"{AppCacheKeys.CombatDetails_ResourcesRecovery}_{i}", currentCombatDetails.ResourcesRecovery);
            _cacheService.SaveDataToCache($"{AppCacheKeys.CombatDetails_ResourcesRecoveryGeneral}_{i}", currentCombatDetails.ResourcesRecoveryGeneral);
        }
    }

    private async Task UploadingCombatLogAsync(List<CombatModel> combatList, Tuple<List<CombatModel>, LogType> dataForGeneralAnalysis)
    {
        var token = ((BasicTemplateViewModel)Basic).RequestCancelationToken();

        Basic.Handler.BasicPropertyUpdate(nameof(BasicTemplateViewModel.ResponseStatus), LoadingStatus.Pending);
        Basic.Handler.BasicPropertyUpdate(nameof(BasicTemplateViewModel.UploadedCombatsCount), 0);

        var createdCombatLog = await _combatParserAPIService.SaveCombatLogAsync(combatList, LogType, CancellationToken.None);
        if (createdCombatLog == null)
        {
            Basic.Handler.BasicPropertyUpdate(nameof(BasicTemplateViewModel.ResponseStatus), LoadingStatus.Failed);

            CombatLogUploadingFailed = true;

            return;
        }

        Basic.Handler.BasicPropertyUpdate(nameof(BasicTemplateViewModel.AllowStep), 1);

        await _mvvmNavigation.Navigate<CombatsViewModel, Tuple<List<CombatModel>, LogType>>(dataForGeneralAnalysis);

        Basic.Handler.PropertyUpdate<CombatsViewModel>(Basic.SavedViewModel, nameof(CombatsViewModel.ResponseStatus), LoadingStatus.Pending);

        Basic.Handler.BasicPropertyUpdate(nameof(BasicTemplateViewModel.Combats), combatList);
        Basic.Handler.BasicPropertyUpdate(nameof(BasicTemplateViewModel.CombatLog), createdCombatLog);

        var combatsAreUploaded = await _combatParserAPIService.SaveAsync(combatList, createdCombatLog, CombatUploaded, token);
        if (!combatsAreUploaded)
        {
            Basic.Handler.BasicPropertyUpdate(nameof(BasicTemplateViewModel.ResponseStatus), LoadingStatus.Failed);

            return;
        }

        var responseStatus = LoadingStatus.Successful;
        Basic.Handler.BasicPropertyUpdate(nameof(BasicTemplateViewModel.ResponseStatus), responseStatus);
    }

    private async Task LoadCombatLogsAsync()
    {
        NoCombatsUploaded = false;

        CombatLogLoadingStatus = LoadingStatus.Pending;

        _combatParserAPIService.SetUpPort();

        var combatLogsData = await _combatParserAPIService.LoadCombatLogsAsync();
        if (combatLogsData == null)
        {
            CombatLogLoadingStatus = LoadingStatus.Failed;
            CombatLogs = new ObservableCollection<CombatLogModel>();

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

        var publicLogs = readyCombatLogData.Where(x => x.LogType == (int)LogType.Public).ToList();
        CombatLogs = new ObservableCollection<CombatLogModel>(publicLogs);

        CombatLogLoadingStatus = LoadingStatus.Successful;

        LoadCombatLogsForTargetUser(readyCombatLogData);
    }

    private void LoadCombatLogsForTargetUser(List<CombatLogModel> combatLogs)
    {
        var user = _memoryCache.Get<AppUserModel>(nameof(MemoryCacheValue.User));
        if (user == null)
        {
            CombatLogsForTargetUser = new ObservableCollection<CombatLogModel>();

            return;
        }

        var combatLogsForTargetUser = combatLogs.Where(x => x.AppUserId == user.Id).ToList();
        CombatLogsForTargetUser = new ObservableCollection<CombatLogModel>(combatLogsForTargetUser);
    }

    private void CombatUploaded(int number, string dungeonName, string name)
    {
        Basic.Handler.PropertyUpdate<CombatsViewModel>(Basic.SavedViewModel, nameof(CombatsViewModel.CurrentCombatNumber), number);
        Basic.Handler.PropertyUpdate<CombatsViewModel>(Basic.SavedViewModel, nameof(CombatsViewModel.DungeonName), dungeonName);
        Basic.Handler.PropertyUpdate<CombatsViewModel>(Basic.SavedViewModel, nameof(CombatsViewModel.Name), name);

        ((BasicTemplateViewModel)Basic).UploadedCombatsCount++;
    }
}
