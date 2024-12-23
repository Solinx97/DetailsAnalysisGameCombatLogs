using CombatAnalysis.Core.Consts;
using CombatAnalysis.Core.Core;
using CombatAnalysis.Core.Enums;
using CombatAnalysis.Core.Helpers;
using CombatAnalysis.Core.Interfaces;
using CombatAnalysis.Core.Interfaces.Observers;
using CombatAnalysis.Core.Models;
using CombatAnalysis.Core.Security;
using CombatAnalysis.Core.ViewModels.Chat;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;

namespace CombatAnalysis.Core.ViewModels.ViewModelTemplates;

public class BasicTemplateViewModel : MvxViewModel, IImprovedMvxViewModel, IVMDataHandler<CombatPlayerModel>, IResponseStatusObservable, IAuthObservable
{
    private readonly List<IResponseStatusObserver> _responseStatusObservers;
    private readonly List<IAuthObserver> _authObservers;
    private readonly IMvxNavigationService _mvvmNavigation;
    private readonly IMemoryCache _memoryCache;
    private readonly SecurityStorage _securityStorage;

    private string? _username;
    private int _step = -1;
    private bool _isRegistrationNotActivated = true;
    private int _uploadingCombatsCount = 1;

    private List<CombatModel>? _combats;
    private CombatModel? _selectedCombat;
    private CombatLogModel? _combatLog;
    private bool _isAuth;
    private bool _loginIsRan;
    private LogType _logType;
    private bool _logPanelStatusIsVisibly;
    private int _uploadedCombatsCount;

    private static LoadingStatus _responseStatus;
    private static int _allowStep;

    public BasicTemplateViewModel(IMvxNavigationService mvvmNavigation, IMemoryCache memoryCache, IHttpClientHelper httpClient, ILogger logger)
    {
        BasicViewModel.Template = this;
        Parent = this;
        SavedViewModel = this;
        Handler = new VMHandler<BasicTemplateViewModel>();

        _securityStorage = new SecurityStorage(memoryCache, httpClient, logger);

        _mvvmNavigation = mvvmNavigation;
        _memoryCache = memoryCache;

        _responseStatusObservers = new List<IResponseStatusObserver>();
        _authObservers = new List<IAuthObserver>();

        CloseCommand = new MvxCommand(CloseWindow);
        LoginCommand = new MvxAsyncCommand(LoginAsync);
        RegistrationCommand = new MvxAsyncCommand(RegistrationAsync);
        LogoutCommand = new MvxAsyncCommand(LogoutAsync);
        ToHomeCommand = new MvxAsyncCommand(ToHomeAsync);
        UploadCombatsCommand = new MvxAsyncCommand(UploadCombatLogsAsync);
        GeneralAnalysisCommand = new MvxAsyncCommand(GeneralAnalysisAsync);
        CombatCommand = new MvxAsyncCommand(DetailsSpecificalCombatAsync);
        LogPanelStatusCommand = new MvxCommand(() => LogPanelStatusIsVisibly = !LogPanelStatusIsVisibly);
        ChatCommand = new MvxAsyncCommand(ChatAsync);
        SettingsCommand = new MvxAsyncCommand(SettingsAsync);

        DamageDoneDetailsCommand = new MvxAsyncCommand(DamageDoneDetailsAsync);
        HealDoneDetailsCommand = new MvxAsyncCommand(HealDoneDetailsAsync);
        DamageTakenDetailsCommand = new MvxAsyncCommand(DamageTakenDetailsAsync);
        ResourceDetailsCommand = new MvxAsyncCommand(ResourceDetailsAsync);

        Task.Run(async () => await _mvvmNavigation.Navigate<HomeViewModel, bool>(IsAuth));
    }

    public event AuthorizationWindowEventHandler? OpenAuthorizationWindow;
    public event AuthorizationWindowEventHandler? OpenRegistrationWindow;
    public event AuthorizationWindowEventHandler? CloseAuthorizationWindow;
    public event AuthorizationWindowEventHandler? CloseRegistrationWindow;

    public IVMHandler Handler { get; set; }

    public IMvxViewModel Parent { get; set; }

    public IMvxViewModel SavedViewModel { get; set; }

    public List<CombatModel> Combats
    {
        set
        {
            if (value != null)
            {
                _combats = value;
                UploadingCombatsCount = value.Count;
            }
        }
        get => _combats ?? new List<CombatModel>();
    }

    public CancellationTokenSource CancellationTokenSource { get; set; } = new();

    #region Commands

    public IMvxCommand CloseCommand { get; set; }

    public IMvxAsyncCommand LoginCommand { get; set; }

    public IMvxAsyncCommand RegistrationCommand { get; set; }

    public IMvxAsyncCommand LogoutCommand { get; set; }

    public IMvxAsyncCommand ToHomeCommand { get; set; }

    public IMvxAsyncCommand UploadCombatsCommand { get; set; }

    public IMvxAsyncCommand GeneralAnalysisCommand { get; set; }

    public IMvxAsyncCommand CombatCommand { get; set; }

    public IMvxAsyncCommand DamageDoneDetailsCommand { get; set; }

    public IMvxAsyncCommand HealDoneDetailsCommand { get; set; }

    public IMvxAsyncCommand DamageTakenDetailsCommand { get; set; }

    public IMvxAsyncCommand ResourceDetailsCommand { get; set; }

    public IMvxCommand LogPanelStatusCommand { get; set; }

    public IMvxAsyncCommand ChatCommand { get; set; }

    public IMvxAsyncCommand SettingsCommand { get; set; }

    #endregion

    #region View model properties

    public CombatPlayerModel Data { get; set; } = new();

    public Dictionary<string, List<string>> PetsId { get; set; } = new();

    public CombatModel? SelectedCombat
    {
        get => _selectedCombat;
        set
        {
            if (value != null)
            {
                _selectedCombat = value;
                DetailsGenericTemplate<DamageDoneModel, DamageDoneGeneralModel>.SelectedCombat = value;
                DetailsGenericTemplate<HealDoneModel, HealDoneGeneralModel>.SelectedCombat = value;
                DetailsGenericTemplate<DamageTakenModel, DamageTakenGeneralModel>.SelectedCombat = value;
                DetailsGenericTemplate<ResourceRecoveryModel, ResourceRecoveryGeneralModel>.SelectedCombat = value;
            }
        }
    }

    public static string? AppVersion
    {
        get { return AppInformation.Version; }
    }

    public static string AppVersionType
    {
        get { return AppInformation.VersionType.ToString(); }
    }

    public int Step
    {
        get { return _step; }
        set
        {
            SetProperty(ref _step, value);
        }
    }

    public int AllowStep
    {
        get { return _allowStep; }
        set
        {
            SetProperty(ref _allowStep, value);
        }
    }

    public LoadingStatus ResponseStatus
    {
        get { return _responseStatus; }
        set
        {
            SetProperty(ref _responseStatus, value);
            NotifyResponseStatusObservers();
        }
    }

    public CombatLogModel? CombatLog
    {
        get { return _combatLog; }
        set
        {
            SetProperty(ref _combatLog, value);
        }
    }

    public bool LoginIsRan
    {
        get { return _loginIsRan; }
        set
        {
            SetProperty(ref _loginIsRan, value);
        }
    }

    public bool IsAuth
    {
        get { return _isAuth; }
        set
        {
            SetProperty(ref _isAuth, value);
            NotifyAuthObservers();
        }
    }

    public bool IsRegistrationNotActivated
    {
        get { return _isRegistrationNotActivated; }
        set
        {
            SetProperty(ref _isRegistrationNotActivated, value);
            NotifyAuthObservers();
        }
    }

    public string? Username
    {
        get { return _username; }
        set
        {
            SetProperty(ref _username, value);
        }
    }

    public LogType LogType
    {
        get { return _logType; }
        set
        {
            SetProperty(ref _logType, value);
        }
    }

    public bool LogPanelStatusIsVisibly
    {
        get { return _logPanelStatusIsVisibly; }
        set
        {
            SetProperty(ref _logPanelStatusIsVisibly, value);
        }
    }

    public int UploadingCombatsCount
    {
        get { return _uploadingCombatsCount; }
        set
        {
            SetProperty(ref _uploadingCombatsCount, value);
        }
    }

    public int UploadedCombatsCount
    {
        get { return _uploadedCombatsCount; }
        set
        {
            SetProperty(ref _uploadedCombatsCount, value);
        }
    }

    #endregion

    public CancellationToken RequestCancelationToken()
    {
        CancellationTokenSource = new CancellationTokenSource();
        var token = CancellationTokenSource.Token;

        return token;
    }

    public void RequestCancel()
    {
        CancellationTokenSource?.Cancel();
    }

    public async Task LoginAsync()
    {
        LoginIsRan = true;

        await _mvvmNavigation.Navigate<HomeViewModel>();
        await AsyncDispatcher.ExecuteOnMainThreadAsync(() =>
        {
            OpenAuthorizationWindow?.Invoke();
        });
    }

    public async Task RegistrationAsync()
    {
        await _mvvmNavigation.Navigate<HomeViewModel>();
        await AsyncDispatcher.ExecuteOnMainThreadAsync(() =>
        {
            OpenRegistrationWindow?.Invoke();
        });
    }

    public async Task LogoutAsync()
    {
        var refreshToken = _memoryCache.Get<string>(nameof(MemoryCacheValue.RefreshToken));
        if (refreshToken == null)
        {
            return;
        }

        _memoryCache.Remove(nameof(MemoryCacheValue.RefreshToken));
        _memoryCache.Remove(nameof(MemoryCacheValue.AccessToken));
        _memoryCache.Remove(nameof(MemoryCacheValue.User));

        IsAuth = false;
        Username = string.Empty;

        _securityStorage.RemoveTokens();

        Step = -1;
        await _mvvmNavigation.Close(Parent);
    }

    public async Task ToHomeAsync()
    {
        Step = -1;
        await _mvvmNavigation.Close(Parent);
        await _mvvmNavigation.Navigate<HomeViewModel, bool>(IsAuth);
    }

    public async Task UploadCombatLogsAsync()
    {
        Step = 0;
        await _mvvmNavigation.Navigate<CombatLogInformationViewModel>();
    }

    public async Task GeneralAnalysisAsync()
    {
        var dataForGeneralAnalysis = Tuple.Create(Combats, LogType);
        await _mvvmNavigation.Navigate<CombatsViewModel, Tuple<List<CombatModel>, LogType>>(dataForGeneralAnalysis);
    }

    public async Task DetailsSpecificalCombatAsync()
    {
        Step = 2;
        await _mvvmNavigation.Close(Parent);
    }

    public async Task DamageDoneDetailsAsync()
    {
        await _mvvmNavigation.Navigate<DamageDoneDetailsViewModel, CombatPlayerModel>(Data);
    }

    public async Task HealDoneDetailsAsync()
    {
        await _mvvmNavigation.Navigate<HealDoneDetailsViewModel, CombatPlayerModel>(Data);
    }

    public async Task DamageTakenDetailsAsync()
    {
        await _mvvmNavigation.Navigate<DamageTakenDetailsViewModel, CombatPlayerModel>(Data);
    }

    public async Task ResourceDetailsAsync()
    {
        await _mvvmNavigation.Navigate<ResourceRecoveryDetailsViewModel, CombatPlayerModel>(Data);
    }

    public async Task ChatAsync()
    {
        await _mvvmNavigation.Navigate<ChatViewModel>();
    }

    public async Task SettingsAsync()
    {
        await _mvvmNavigation.Navigate<SettingsViewModel>();
    }

    public void AddObserver(IResponseStatusObserver o)
    {
        _responseStatusObservers.Add(o);
    }

    public void RemoveObserver(IResponseStatusObserver o)
    {
        _responseStatusObservers.Remove(o);
    }

    public void NotifyResponseStatusObservers()
    {
        foreach (var item in _responseStatusObservers)
        {
            item.Update(ResponseStatus);
        }
    }

    public void AddObserver(IAuthObserver o)
    {
        _authObservers.Add(o);
    }

    public void RemoveObserver(IAuthObserver o)
    {
        _authObservers.Remove(o);
    }

    public void NotifyAuthObservers()
    {
        foreach (var item in _authObservers)
        {
            item.AuthUpdate(IsAuth);
        }
    }

    public void ClearEvents()
    {
        OpenAuthorizationWindow = null;
        CloseAuthorizationWindow = null;
        OpenRegistrationWindow = null;
        CloseRegistrationWindow = null;
    }

    private void CloseWindow()
    {
        RequestCancel();

        Environment.Exit(0);
    }

    public delegate void AuthorizationWindowEventHandler();
}