using AutoMapper;
using CombatAnalysis.CombatParser.Interfaces;
using CombatAnalysis.Core.Consts;
using CombatAnalysis.Core.Enums;
using CombatAnalysis.Core.Interfaces;
using CombatAnalysis.Core.Models.GameLogs;
using CombatAnalysis.Core.ViewModels.Base;
using CombatAnalysis.Core.ViewModels.ViewModelTemplates;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using System.Collections.ObjectModel;

namespace CombatAnalysis.Core.ViewModels.CombatLogs;

public class ParsingCombatLogsViewModel : ParentTemplate
{
    private readonly IMvxNavigationService _mvvmNavigation;
    private readonly IMapper _mapper;
    private readonly ICombatParserService _parser;
    private readonly ICombatParserAPIService _combatParserAPIService;

    private ObservableCollection<string> _combatLogNames = [];
    private string? _dungeonName;
    private string? _combatName;
    private ObservableCollection<string> _combatLogPaths = [];
    private ObservableCollection<CombatLogModel> _combatLogs = [];
    private ObservableCollection<CombatLogModel> _combatLogsForTargetUser = [];
    private bool _isAllowSaveLogs = true;
    private CancellationTokenSource _cancellationTokenSource = new();

    private bool _fileIsCorrect = true;
    private bool _isParsing;
    private bool _combatLogUploadingFailed;
    private bool _isAuth;
    private LogType _logType;
    private LoadingStatus _combatLogLoadingStatus;
    private bool _uploadingLogs;
    private bool _noCombatsUploaded;
    private bool _processAborted;
    private bool _showConnectMore;

    public ParsingCombatLogsViewModel(IMapper mapper, IMvxNavigationService mvvmNavigation, ICombatParserService parser,
        ICombatParserAPIService combatParserAPIService)
    {
        _mapper = mapper;
        _mvvmNavigation = mvvmNavigation;
        _parser = parser;
        _combatParserAPIService = combatParserAPIService;

        OpenPlayerAnalysisCommand = new MvxAsyncCommand(OpenPlayerAnalysisAsync);
        CancelParsingCommand = new MvxCommand(CancelParsing);

        GetLogTypeCommand = new MvxCommand<int>(GetLogType);

        Basic.Parent = this;
        Basic.Handler.BasicPropertyUpdate(nameof(BasicTemplateViewModel.Step), 0);
        Basic.Handler.BasicPropertyUpdate(nameof(BasicTemplateViewModel.LogPanelStatusIsVisibly), true);
    }

    #region Commands

    public IMvxAsyncCommand OpenPlayerAnalysisCommand { get; private set; }

    public IMvxCommand<int> GetLogTypeCommand { get; private set; }

    public IMvxCommand CancelParsingCommand { get; private set; }

    #endregion

    #region View model properties

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

    public ObservableCollection<string> CombatLogNames
    {
        get { return _combatLogNames; }
        set
        {
            SetProperty(ref _combatLogNames, value);
        }
    }

    public ObservableCollection<string> CombatLogPaths
    {
        get { return _combatLogPaths; }
        set
        {
            SetProperty(ref _combatLogPaths, value);

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

    public bool FileIsCorrect
    {
        get { return _fileIsCorrect; }
        set
        {
            SetProperty(ref _fileIsCorrect, value);
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

    public bool ShowConnectMore
    {
        get { return _showConnectMore; }
        set
        {
            SetProperty(ref _showConnectMore, value);
        }
    }

    #endregion

    #region Ovveride methods

    public override void Prepare()
    {
        base.Prepare();

        CombatLogPaths = [.. AppStaticData.SelectedCombatLogFilePaths];
        CombatLogPaths.CollectionChanged += CombatLogPaths_CollectionChanged;

        ShowConnectMore = CombatLogPaths.Count > 0;
        GetCombatLogNames();
    }

    #endregion

    private void CombatLogPaths_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
    {
        GetCombatLogNames();

        ShowConnectMore = CombatLogPaths.Count > 0;
    }

    private void GetLogType(int logType)
    {
        LogType = (LogType)logType;
    }

    private void GetCombatLogNames()
    {
        if (CombatLogPaths.Count == 0)
        {
            return;
        }

        CombatLogNames.Clear();

        foreach (var item in CombatLogPaths)
        {
            var split = item.Split(@"\");
            CombatLogNames.Add(split[^1]);
        }
    }

    private async Task OpenPlayerAnalysisAsync()
    {
        CombatLogUploadingFailed = false;
        Basic.Handler.BasicPropertyUpdate(nameof(BasicTemplateViewModel.ResponseStatus), LoadingStatus.None);

        await CombatLogFileValidateAsync(CombatLogPaths.ToList() ?? []);
    }

    private void CancelParsing()
    {
        _processAborted = true;
        _cancellationTokenSource?.Cancel();
    }

    private async Task CombatLogFileValidateAsync(List<string> combatLogPaths)
    {
        foreach (var item in combatLogPaths)
        {
            FileIsCorrect = await _parser.FileCheckAsync(item);
            if (!FileIsCorrect) return;
        }

        IsParsing = true;

        await PrepareCombatDataAsync(combatLogPaths);

        IsParsing = false;
    }

    private async Task PrepareCombatDataAsync(List<string> combatLogPaths)
    {
        _cancellationTokenSource = new CancellationTokenSource();

        Basic.Handler.BasicPropertyUpdate(nameof(BasicTemplateViewModel.Combats), new List<CombatModel>());
        Basic.Handler.BasicPropertyUpdate(nameof(BasicTemplateViewModel.PetsId), new Dictionary<string, List<string>>());
        Basic.Handler.BasicPropertyUpdate(nameof(BasicTemplateViewModel.AllowStep), 0);

        CombatParser.Consts.API.CombatParserApi = API.CombatParserApi;
        await Task.Run(() => _parser.ParseAsync(combatLogPaths, _cancellationTokenSource.Token));

        var combats = _mapper.Map<List<CombatModel>>(_parser.Combats);
        await _combatParserAPIService.GetBossAsync(combats, _cancellationTokenSource.Token);

        AppStaticData.PreparedCombatsCount = _parser.Combats.Count;

        _parser.Clear();

        if (_processAborted)
        {
            _processAborted = false;

            return;
        }

        await UploadingCombatLogAsync(combats, combats);
    }

    private async Task UploadingCombatLogAsync(List<CombatModel> combatList, List<CombatModel> combats)
    {
        var token = ((BasicTemplateViewModel)Basic).RequestCancelationToken();
        var createdCombatLog = await _combatParserAPIService.SaveCombatLogAsync(combatList, LogType, token);
        if (createdCombatLog.AppUserId == null)
        {
            Basic.Handler.BasicPropertyUpdate(nameof(BasicTemplateViewModel.ResponseStatus), LoadingStatus.Failed);

            CombatLogUploadingFailed = true;

            return;
        }

        Basic.Handler.BasicPropertyUpdate(nameof(BasicTemplateViewModel.Combats), combatList);
        Basic.Handler.BasicPropertyUpdate(nameof(BasicTemplateViewModel.CombatLog), createdCombatLog);

        Basic.Handler.BasicPropertyUpdate(nameof(BasicTemplateViewModel.AllowStep), 1);

        Basic.Handler.BasicPropertyUpdate(nameof(BasicTemplateViewModel.IsCombatLogsMustSave), true);

        await _mvvmNavigation.Navigate<CombatsViewModel, List<CombatModel>>(combats);
    }
}
