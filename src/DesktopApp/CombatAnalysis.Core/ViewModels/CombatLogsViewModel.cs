using CombatAnalysis.Core.Enums;
using CombatAnalysis.Core.Interfaces;
using CombatAnalysis.Core.Interfaces.Observers;
using CombatAnalysis.Core.Models.User;
using CombatAnalysis.Core.ViewModels.Base;
using CombatAnalysis.Core.ViewModels.CombatLogs;
using CombatAnalysis.Core.ViewModels.ViewModelTemplates;
using Microsoft.Extensions.Caching.Memory;
using MvvmCross.Navigation;

namespace CombatAnalysis.Core.ViewModels;

public class CombatLogsViewModel : ParentTemplate, IAuthObserver
{
    private readonly IMemoryCache _memoryCache;

    private int _selectedTabIndex;
    private bool _isAllowSwitchTabs = true;
    private bool _isAuth;

    public CombatLogsViewModel(IMvxNavigationService mvvmNavigation, IMemoryCache memoryCache, 
        ICombatParserAPIService combatParserAPIService)
    {
        _memoryCache = memoryCache;

        PublicCombatLogsVM = new PublicCombatLogsViewModel(mvvmNavigation, combatParserAPIService, memoryCache);
        PrivateCombatLogsVM = new PrivateCombatLogsViewModel(mvvmNavigation, combatParserAPIService, memoryCache);

        Basic.Parent = this;
        Basic.Handler.BasicPropertyUpdate(nameof(BasicTemplateViewModel.Step), 0);
        Basic.Handler.BasicPropertyUpdate(nameof(BasicTemplateViewModel.LogPanelStatusIsVisibly), true);

        var authObservable = Basic as IAuthObservable;
        authObservable?.AddObserver(this);

        IsAuth = false;
    }

    #region View model properties

    public int SelectedTabIndex
    {
        get { return _selectedTabIndex; }
        set
        {
            SetProperty(ref _selectedTabIndex, value);
        }
    }

    public bool IsAllowSwitchTabs
    {
        get { return _isAllowSwitchTabs; }
        set
        {
            SetProperty(ref _isAllowSwitchTabs, value);
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

    #endregion

    public PublicCombatLogsViewModel PublicCombatLogsVM { get; }

    public PrivateCombatLogsViewModel PrivateCombatLogsVM { get; }

    public void AuthUpdate(bool isAuth)
    {
        IsAuth = isAuth;
    }

    #region Ovveride methods

    public override async Task Initialize()
    {
        await PublicCombatLogsVM.Initialize();
        await PrivateCombatLogsVM.Initialize();

        await base.Initialize();
    }

    public override void ViewAppeared()
    {
        CheckAuth();
    }

    #endregion

    private void CheckAuth()
    {
        var user = _memoryCache.Get<AppUserModel>(nameof(MemoryCacheValue.User));
        IsAuth = user != null;
    }
}
