using CombatAnalysis.Core.Interfaces.Observers;
using CombatAnalysis.Core.ViewModels.Base;
using CombatAnalysis.Core.ViewModels.Chat;
using CombatAnalysis.Core.ViewModels.User;
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

        BasicTemplate = ParentTemplate.Basic;
        BasicTemplate.Handler.PropertyUpdate<BasicTemplateViewModel>(BasicTemplate, nameof(BasicTemplateViewModel.Step), -1);

        var authObservable = (IAuthObservable)BasicTemplate;
        authObservable.AddObserver(this);
    }

    #region Command

    public IMvxAsyncCommand OpenChatCommand { get; set; }

    public IMvxAsyncCommand OpenLognCommand { get; set; }

    public IMvxAsyncCommand OpenCombatAnalysisCommand { get; set; }

    #endregion

    #region Properties

    public bool ChatIsEnabled
    {
        get { return _chatIsEnabled; }
        set
        {
            SetProperty(ref _chatIsEnabled, value);
        }
    }

    #endregion

    protected override void ChildPrepare(bool isAuth)
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
