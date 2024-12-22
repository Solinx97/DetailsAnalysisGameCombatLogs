using CombatAnalysis.Core.Interfaces.Observers;
using CombatAnalysis.Core.ViewModels.Base;
using CombatAnalysis.Core.ViewModels.Chat;
using CombatAnalysis.Core.ViewModels.User;
using CombatAnalysis.Core.ViewModels.ViewModelTemplates;
using MvvmCross.Commands;
using MvvmCross.Navigation;

namespace CombatAnalysis.Core.ViewModels;

public class HomeViewModel : ParentTemplate<bool>, IAuthObserver
{
    private readonly IMvxNavigationService _mvvmNavigation;

    private bool _chatIsEnabled;

    public HomeViewModel(IMvxNavigationService mvvmNavigation)
    {
        _mvvmNavigation = mvvmNavigation;

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

    public bool ChatIsEnabled
    {
        get { return _chatIsEnabled; }
        set
        {
            SetProperty(ref _chatIsEnabled, value);
        }
    }

    #endregion

    public override void Prepare(bool isAuth)
    {
        ChatIsEnabled = isAuth;
    }

    public async Task OpenChatAsync()
    {
        await _mvvmNavigation.Navigate<ChatViewModel>();
    }

    public async Task OpenLoginAsync()
    {
        await _mvvmNavigation.Navigate<AuthorizationViewModel>();
    }

    public async Task OpenCombatAnalysisAsync()
    {
        await _mvvmNavigation.Navigate<CombatLogInformationViewModel>();
    }

    public void AuthUpdate(bool isAuth)
    {
        ChatIsEnabled = isAuth;
    }
}
