using CombatAnalysis.Core.Enums;
using CombatAnalysis.Core.Interfaces;
using CombatAnalysis.Core.Interfaces.Observers;
using CombatAnalysis.Core.ViewModels.Base;
using CombatAnalysis.Core.ViewModels.Chat;
using CombatAnalysis.Core.ViewModels.User;
using CombatAnalysis.Core.ViewModels.ViewModelTemplates;
using MvvmCross.Commands;
using MvvmCross.Navigation;

namespace CombatAnalysis.Core.ViewModels;

public class HomeViewModel : ParentTemplate<AuthAction>, IAuthObserver
{
    private readonly IMvxNavigationService _mvvmNavigation;
    private readonly IAuthWindowService<AuthorizationViewModel> _loginWindowService;
    private readonly IAuthWindowService<RegistrationViewModel> _registrationWindowService;

    private bool _isAuth;
    private AuthAction _authAction = AuthAction.None;

    public HomeViewModel(IMvxNavigationService mvvmNavigation, IAuthWindowService<AuthorizationViewModel> loginWindowService, IAuthWindowService<RegistrationViewModel> registrationWindowService)
    {
        _mvvmNavigation = mvvmNavigation;
        _loginWindowService = loginWindowService;
        _registrationWindowService = registrationWindowService;

        OpenChatCommand = new MvxAsyncCommand(OpenChatAsync);
        OpenLognCommand = new MvxAsyncCommand(OpenLoginAsync);
        OpenCombatAnalysisCommand = new MvxAsyncCommand(OpenCombatAnalysisAsync);

        Basic.Handler.BasicPropertyUpdate(nameof(BasicTemplateViewModel.Step), -1);

        var authObservable = Basic as IAuthObservable;
        authObservable?.AddObserver(this);
    }

    #region Command

    public IMvxAsyncCommand OpenChatCommand { get; set; }

    public IMvxAsyncCommand OpenLognCommand { get; set; }

    public IMvxAsyncCommand OpenCombatAnalysisCommand { get; set; }

    #endregion

    #region View model properties

    public bool IsAuth
    {
        get { return _isAuth; }
        set
        {
            SetProperty(ref _isAuth, value);
        }
    }

    #endregion

    public override Task Initialize()
    {
        IsAuth = ((BasicTemplateViewModel)Basic).IsAuth;
        return base.Initialize();
    }

    public override void ViewAppeared()
    {
        switch (_authAction)
        {
            case AuthAction.Login:
                AsyncDispatcher.ExecuteOnMainThreadAsync(_loginWindowService.ShowAsync);
                break;
            case AuthAction.Registration:
                AsyncDispatcher.ExecuteOnMainThreadAsync(_registrationWindowService.ShowAsync);
                break;
        }
    }

    public override void Prepare(AuthAction triggerAuth)
    {
        _authAction = triggerAuth;
    }

    public async Task OpenChatAsync()
    {
        await _mvvmNavigation.Navigate<ChatViewModel>();
    }

    public async Task OpenLoginAsync()
    {
        if (Basic is BasicTemplateViewModel basicTemplate)
        {
            await basicTemplate.LoginAsync();
        }
    }

    public async Task OpenCombatAnalysisAsync()
    {
        await _mvvmNavigation.Navigate<CombatLogsViewModel>();
    }

    public void AuthUpdate(bool isAuth)
    {
        IsAuth = isAuth;
    }
}
