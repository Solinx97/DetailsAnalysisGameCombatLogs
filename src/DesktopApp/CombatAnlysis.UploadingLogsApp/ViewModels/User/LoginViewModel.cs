using CombatAnalysis.UploadingLogsApp.Core;
using CombatAnalysis.UploadingLogsApp.Enums;
using CombatAnalysis.UploadingLogsApp.Interfaces;
using CombatAnalysis.UploadingLogsApp.Interfaces.Security;
using CombatAnalysis.UploadingLogsApp.Models.User;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CombatAnalysis.UploadingLogsApp.ViewModels.User;

public partial class LoginViewModel : ViewModelBase, IAsyncInitializable
{
    private readonly AppState _appState;
    private readonly INavigationService _navigationService;
    private readonly IMemoryCache _memoryCache;
    private readonly IIdentityService _identityService;
    private readonly ISecurityStorage _securityStorage;
    private readonly ILogger<LoginViewModel> _logger;

    private CancellationTokenSource _cts = new();

    public LoginViewModel()
    {
    }

    public LoginViewModel(AppState appState, INavigationService navigationService, IMemoryCache memoryCache, 
        IIdentityService identityService, ISecurityStorage securityStorage, ILogger<LoginViewModel> logger)
    {
        _appState = appState;
        _navigationService = navigationService;
        _memoryCache = memoryCache;
        _identityService = identityService;
        _securityStorage = securityStorage;
        _logger = logger;
    }

    #region View model properties

    [ObservableProperty]
    public partial bool CheckAuthInProgress { get; set; }

    [ObservableProperty]
    public partial bool AuthInProgress { get; set; }

    [ObservableProperty]
    public partial bool AuthIsFailed { get; set; }

    [ObservableProperty]
    public partial bool VerificationInProgress { get; set; }

    [ObservableProperty]
    public partial bool AbortAvailable { get; set; }

    #endregion

    [RelayCommand]
    public async Task SendAuthorizationRequest()
    {
        try
        {
            AuthIsFailed = false;
            AbortAvailable = true;
            AuthInProgress = true;

            await _identityService.SendAuthorizationRequestAsync("connect/authorize", _cts.Token);
            _cts.Token.ThrowIfCancellationRequested();

            VerificationInProgress = true;

            await _identityService.SendTokenRequestAsync(_cts.Token);
            _cts.Token.ThrowIfCancellationRequested();

            VerificationInProgress = false;
            AbortAvailable = false;
            AuthInProgress = false;

            var user = _memoryCache.Get<AppUserModel>(nameof(MemoryCacheValue.User));
            if (user != null)
            {
                _appState.IsAuth = true;
                _appState.User = user;

                await  _navigationService.NavigateTo<ParsingCombatLogsViewModel>();
            }
            else
            {
                AuthIsFailed = true;
            }
        }
        catch (OperationCanceledException ex)
        {
            _logger.LogInformation("Listening cancelled");

            VerificationInProgress = false;
            AuthIsFailed = true;
            AbortAvailable = false;
            AuthInProgress = false;
        }
    }

    [RelayCommand]
    public void Abort()
    {
        _cts.Cancel();

        _appState.IsAuth = false;
        CheckAuthInProgress = false;
        VerificationInProgress = false;
        AuthIsFailed = true;
        AbortAvailable = false;

        _cts = new();
    }

    public async Task InitializeAsync()
    {
        await CheckAuthAsync();
    }

    private async Task CheckAuthAsync()
    {
        try
        {
            CheckAuthInProgress = true;
            AbortAvailable = true;

            var user = await _securityStorage.GetUserAsync(_cts.Token);
            _cts.Token.ThrowIfCancellationRequested();

            if (user == null)
            {
                AuthIsFailed = true;
                CheckAuthInProgress = false;
                AbortAvailable = false;

                return;
            }

            _securityStorage.GetAccessToken();

            _appState.IsAuth = true;
            _appState.User = user;

            await _navigationService.NavigateTo<ParsingCombatLogsViewModel>();
        }
        catch (OperationCanceledException)
        {
            _logger.LogInformation("Listening cancelled");

            CheckAuthInProgress = false;
            AuthIsFailed = true;
            AbortAvailable = false;
        }
    }
}
