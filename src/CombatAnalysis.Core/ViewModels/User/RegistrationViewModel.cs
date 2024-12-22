using CombatAnalysis.Core.Enums;
using CombatAnalysis.Core.Interfaces;
using CombatAnalysis.Core.Models.User;
using CombatAnalysis.Core.ViewModels.Base;
using CombatAnalysis.Core.ViewModels.ViewModelTemplates;
using Microsoft.Extensions.Caching.Memory;
using MvvmCross.Navigation;

namespace CombatAnalysis.Core.ViewModels.User;

public class RegistrationViewModel : ParentTemplate
{
    private readonly IMemoryCache _memoryCache;
    private readonly IMvxNavigationService _mvvmNavigation;
    private readonly IIdentityService _identityService;

    private bool _isVerification;

    public RegistrationViewModel(IMemoryCache memoryCache, IMvxNavigationService mvvmNavigation, IIdentityService identityService)
    {
        _memoryCache = memoryCache;
        _mvvmNavigation = mvvmNavigation;
        _identityService = identityService;

        if (Basic.Parent is AuthorizationViewModel)
        {
            _mvvmNavigation.Close(Basic.Parent).GetAwaiter().GetResult();
        }

        Basic.Handler.BasicPropertyUpdate(nameof(BasicTemplateViewModel.IsLoginNotActivated), true);
        Basic.Parent = this;
    }

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

    public override void ViewDisappeared()
    {
        Basic.Handler.BasicPropertyUpdate(nameof(BasicTemplateViewModel.IsLoginNotActivated), true);

        base.ViewDisappeared();
    }

    public override void ViewAppeared()
    {
        Task.Run(SendAuthorizationRequestAsync);
    }

    private async Task SendAuthorizationRequestAsync()
    {
        await _identityService.SendAuthorizationRequestAsync("registration");

        IsVerification = true;

        await _identityService.SendTokenRequestAsync();

        var user = _memoryCache.Get<AppUserModel>(nameof(MemoryCacheValue.User));
        if (user != null)
        {
            Basic.Handler.BasicPropertyUpdate(nameof(BasicTemplateViewModel.IsAuth), true);
            Basic.Handler.BasicPropertyUpdate(nameof(BasicTemplateViewModel.Username), user.Username);
        }

        await _mvvmNavigation.Close(this);
    }
}
