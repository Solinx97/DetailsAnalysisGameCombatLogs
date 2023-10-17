using CombatAnalysis.Core.Consts;
using CombatAnalysis.Core.Core;
using CombatAnalysis.Core.Enums;
using CombatAnalysis.Core.Helpers;
using CombatAnalysis.Core.Interfaces;
using CombatAnalysis.Core.Interfaces.Observers;
using CombatAnalysis.Core.Models;
using CombatAnalysis.Core.Models.User;
using CombatAnalysis.Core.ViewModels.Base;
using CombatAnalysis.Core.ViewModels.Chat;
using CombatAnalysis.Core.ViewModels.ViewModelTemplates;
using Microsoft.Extensions.Caching.Memory;
using MvvmCross.Commands;
using MvvmCross.Navigation;

namespace CombatAnalysis.Core.ViewModels;

public class BasicTemplateViewModel : ParentTemplate, IVMDataHandler<CombatPlayerModel>, IResponseStatusObservable, IAuthObservable
{
    private readonly List<IResponseStatusObserver> _responseStatusObservers;
    private readonly List<IAuthObserver> _authObservers;
    private readonly IMvxNavigationService _mvvmNavigation;
    private readonly IMemoryCache _memoryCache;
    private readonly IHttpClientHelper _httpClient;

    private int _step = -1;
    private List<CombatModel> _combats;
    private bool _isAuth;
    private bool _isLoginNotActivated = true;
    private bool _isRegistrationNotActivated = true;
    private string _username;
    private LogType _logType;
    private bool _logPanelStatusIsVisibly;
    private CombatModel _selectedCombat;

    private static LoadingStatus _responseStatus;
    private static int _allowStep;

    public BasicTemplateViewModel(IMvxNavigationService mvvmNavigation, IMemoryCache memoryCache, IHttpClientHelper httpClient)
    {
        Handler = new VMHandler();

        _mvvmNavigation = mvvmNavigation;
        _memoryCache = memoryCache;
        _httpClient = httpClient;

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

        Basic = this;

        CheckAuth();
        Task.Run(async () => await _mvvmNavigation.Navigate<HomeViewModel, bool>(IsAuth));
    }

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

    #region Properties

    public CombatPlayerModel Data { get; set; }

    public Dictionary<string, List<string>> PetsId { get; set; }

    public CombatModel SelectedCombat
    {
        get
        {
            return _selectedCombat;
        }

        set
        {
            _selectedCombat = value;
            DetailsGenericTemplate<DamageDoneModel, DamageDoneGeneralModel>.SelectedCombat = value;
            DetailsGenericTemplate<HealDoneModel, HealDoneGeneralModel>.SelectedCombat = value;
            DetailsGenericTemplate<DamageTakenModel, DamageTakenGeneralModel>.SelectedCombat = value;
            DetailsGenericTemplate<ResourceRecoveryModel, ResourceRecoveryGeneralModel>.SelectedCombat = value;
        }
    }

    public string AppVersion
    {
        get { return AppInformation.Version; }
    }

    public string AppVersionType
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

    public List<CombatModel> Combats
    {
        get { return _combats; }
        set
        {
            SetProperty(ref _combats, value);
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

    public bool IsLoginNotActivated
    {
        get { return _isLoginNotActivated; }
        set
        {
            SetProperty(ref _isLoginNotActivated, value);
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

    public string Username
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

    #endregion

    public void CloseWindow()
    {
        Environment.Exit(0);
    }

    public async Task LoginAsync()
    {
        IsLoginNotActivated = false;
        await _mvvmNavigation.Navigate<LoginViewModel>();
    }

    public async Task RegistrationAsync()
    {
        IsRegistrationNotActivated = false;
        await _mvvmNavigation.Navigate<RegistrationViewModel>();
    }

    public async Task LogoutAsync()
    {
        var refreshToken = _memoryCache.Get<string>(nameof(MemoryCacheValue.RefreshToken));

        _httpClient.BaseAddress = Port.UserApi;
        await _httpClient.GetAsync($"Account/logout/{refreshToken}");

        _memoryCache.Remove("refreshToken");
        _memoryCache.Remove("accessToken");
        _memoryCache.Remove("account");

        IsAuth = false;
        Username = string.Empty;

        if (Parent is ChatViewModel)
        {
            Step = 0;
            await _mvvmNavigation.Close(Parent);
        }
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
        await _mvvmNavigation.Navigate<GeneralAnalysisViewModel, Tuple<List<CombatModel>, LogType>>(dataForGeneralAnalysis);
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
        Step = -3;
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

    public void CheckAuth()
    {
        var customer = _memoryCache.Get<CustomerModel>(nameof(MemoryCacheValue.Customer));
        if (customer == null)
        {
            return;
        }

        IsAuth = true;
        Username = customer.Username;
    }
}