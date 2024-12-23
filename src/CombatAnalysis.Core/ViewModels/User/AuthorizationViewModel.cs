using CombatAnalysis.Core.Enums;
using CombatAnalysis.Core.Interfaces;
using CombatAnalysis.Core.Models.User;
using CombatAnalysis.Core.ViewModels.Base;
using CombatAnalysis.Core.ViewModels.ViewModelTemplates;
using Microsoft.Extensions.Caching.Memory;
using MvvmCross;

namespace CombatAnalysis.Core.ViewModels.User;

public class AuthorizationViewModel : ParentTemplate
{
    private readonly IMemoryCache _memoryCache;
    private readonly IIdentityService _identityService;

    private bool _isVerification;

    public AuthorizationViewModel(IMemoryCache memoryCache, IIdentityService identityService)
    {
        _memoryCache = memoryCache;
        _identityService = identityService;

        Basic.Handler.BasicPropertyUpdate(nameof(BasicTemplateViewModel.IsRegistrationNotActivated), true);
        Basic.Parent = this;
    }

    public event CloseAuthorizationWindowEventHandler? CloseAuthorizationWindow;

    #region View model properties

    public bool IsVerification
    {
        get { return _isVerification; }
        set
        {
            SetProperty(ref _isVerification, value);
        }
    }

    #endregion

    public void SendRequest()
    {
        Task.Run(SendAuthorizationRequestAsync);
    }

    private async Task SendAuthorizationRequestAsync()
    {
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

        Basic.Handler.BasicPropertyUpdate(nameof(BasicTemplateViewModel.AuthorizationIsOpen), false);
    }

    public delegate void CloseAuthorizationWindowEventHandler();
}
