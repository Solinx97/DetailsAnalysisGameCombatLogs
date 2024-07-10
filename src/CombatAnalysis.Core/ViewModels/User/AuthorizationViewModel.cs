using CombatAnalysis.Core.Enums;
using CombatAnalysis.Core.Interfaces;
using CombatAnalysis.Core.Models.User;
using CombatAnalysis.Core.ViewModels.Base;
using Microsoft.Extensions.Caching.Memory;
using MvvmCross.Navigation;

namespace CombatAnalysis.Core.ViewModels.User;

public class AuthorizationViewModel : ParentTemplate
{
    private readonly IMemoryCache _memoryCache;
    private readonly IMvxNavigationService _mvvmNavigation;
    private readonly IIdentityService _identityService;

    private bool _isVerification;

    public AuthorizationViewModel(IMemoryCache memoryCache, IMvxNavigationService mvvmNavigation, IIdentityService identityService)
    {
        _memoryCache = memoryCache;
        _mvvmNavigation = mvvmNavigation;
        _identityService = identityService;

        if (BasicTemplate.Parent is RegistrationViewModel)
        {
            Task.Run(async () => await _mvvmNavigation.Close(BasicTemplate.Parent));
        }

        BasicTemplate.Handler.PropertyUpdate<BasicTemplateViewModel>(BasicTemplate, "IsRegistrationNotActivated", true);
        BasicTemplate.Parent = this;
    }

    public bool IsVerification
    {
        get { return _isVerification; }
        set
        {
            SetProperty(ref _isVerification, value);
        }
    }

    public override void ViewDisappeared()
    {
        BasicTemplate.Handler.PropertyUpdate<BasicTemplateViewModel>(BasicTemplate, nameof(BasicTemplateViewModel.IsLoginNotActivated), true);

        base.ViewDisappeared();
    }

    public override void ViewAppeared()
    {
        Task.Run(SendAuthorizationRequestAsync);
    }

    private async Task SendAuthorizationRequestAsync()
    {
        await _identityService.SendAuthorizationRequestAsync("authorization");

        IsVerification = true;

        await _identityService.SendTokenRequestAsync();

        var customer = _memoryCache.Get<CustomerModel>(nameof(MemoryCacheValue.Customer));
        if (customer != null)
        {
            BasicTemplate.Handler.PropertyUpdate<BasicTemplateViewModel>(BasicTemplate, nameof(BasicTemplateViewModel.IsAuth), true);
            BasicTemplate.Handler.PropertyUpdate<BasicTemplateViewModel>(BasicTemplate, nameof(BasicTemplateViewModel.Username), customer.Username);
        }

        await _mvvmNavigation.Close(this);
    }
}
