using CombatAnalysis.Core.Enums;
using CombatAnalysis.Core.Interfaces;
using CombatAnalysis.Core.Models.User;
using CombatAnalysis.Core.Security;
using CombatAnalysis.Core.ViewModels.Base;
using CombatAnalysis.Core.ViewModels.ViewModelTemplates;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using MvvmCross.Commands;

namespace CombatAnalysis.Core.ViewModels.User;

public class AuthorizationViewModel : ParentTemplate
{
    private readonly IMemoryCache _memoryCache;
    private readonly IIdentityService _identityService;
    private readonly SecurityStorage _securityStorage;

    private bool _checkAuthIsRan;
    private bool _isVerification;
    private bool _authorizationIsRan;

    public AuthorizationViewModel(IMemoryCache memoryCache, IIdentityService identityService, IHttpClientHelper httpClient, ILogger logger)
    {
        _memoryCache = memoryCache;
        _identityService = identityService;

        _securityStorage = new SecurityStorage(memoryCache, httpClient, logger);

        LoginCommand = new MvxAsyncCommand(SendAuthorizationRequestAsync);

        Basic.Handler.BasicPropertyUpdate(nameof(BasicTemplateViewModel.IsRegistrationNotActivated), true);
        Basic.Parent = this;

        RunCheckAuth();
    }

    public event CloseAuthorizationWindowEventHandler? CloseAuthorizationWindow;

    #region Commands

    public IMvxAsyncCommand LoginCommand { get; set; }

    #endregion

    #region View model properties

    public bool CheckAuthIsRan
    {
        get { return _checkAuthIsRan; }
        set
        {
            SetProperty(ref _checkAuthIsRan, value);
        }
    }

    public bool IsVerification
    {
        get { return _isVerification; }
        set
        {
            SetProperty(ref _isVerification, value);
        }
    }

    public bool AuthorizationIsRan
    {
        get { return _authorizationIsRan; }
        set
        {
            SetProperty(ref _authorizationIsRan, value);
        }
    }

    #endregion

    private void RunCheckAuth()
    {
        var basicViewModel = (BasicTemplateViewModel)Basic;

        if (basicViewModel.LoginIsRan)
        {
            Task.Run(SendAuthorizationRequestAsync);
        }
        else
        {
            Task.Run(CheckAuthAsync);
        }
    }

    private async Task CheckAuthAsync()
    {
        CheckAuthIsRan = true;

        var user = await _securityStorage.GetUserAsync();
        if (user == null)
        {
            CheckAuthIsRan = false;

            return;
        }

        Basic.Handler.BasicPropertyUpdate(nameof(BasicTemplateViewModel.Username), user.Username);
        Basic.Handler.BasicPropertyUpdate(nameof(BasicTemplateViewModel.IsAuth), true);

        await AsyncDispatcher.ExecuteOnMainThreadAsync(() =>
        {
            CloseAuthorizationWindow?.Invoke();
        });
    }

    private async Task SendAuthorizationRequestAsync()
    {
        AuthorizationIsRan = true;

        await _identityService.SendAuthorizationRequestAsync("authorization");

        IsVerification = true;

        await _identityService.SendTokenRequestAsync();

        var user = _memoryCache.Get<AppUserModel>(nameof(MemoryCacheValue.User));
        if (user != null)
        {
            Basic.Handler.BasicPropertyUpdate(nameof(BasicTemplateViewModel.IsAuth), true);
            Basic.Handler.BasicPropertyUpdate(nameof(BasicTemplateViewModel.Username), user.Username);
        }

        await AsyncDispatcher.ExecuteOnMainThreadAsync(() =>
        {
            CloseAuthorizationWindow?.Invoke();
        });
    }
}

public delegate void CloseAuthorizationWindowEventHandler();
