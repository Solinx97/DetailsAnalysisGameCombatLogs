using CombatAnalysis.Core.Consts;
using CombatAnalysis.Core.Interfaces;
using CombatAnalysis.Core.Interfaces.Observers;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;

namespace CombatAnalysis.Core.ViewModels;

public class HomeViewModel : MvxViewModel<bool>, IAuthObserver
{
    private readonly IMvxNavigationService _mvvmNavigation;

    private IImprovedMvxViewModel _basicTemplate;
    private bool _chatIsEnabled;

    public HomeViewModel(IMvxNavigationService mvvmNavigation)
    {
        _mvvmNavigation = mvvmNavigation;

        OpenChatCommand = new MvxAsyncCommand(OpenChatAsync);
        OpenCombatAnalysisCommand = new MvxAsyncCommand(OpenCombatAnalysisAsync);

        BasicTemplate = Templates.Basic;
        BasicTemplate.Handler.PropertyUpdate<BasicTemplateViewModel>(BasicTemplate, nameof(BasicTemplateViewModel.Step), -1);

        var authObservable = (IAuthObservable)BasicTemplate;
        authObservable.AddObserver(this);
    }

    #region Command

    public IMvxAsyncCommand OpenChatCommand { get; set; }

    public IMvxAsyncCommand OpenCombatAnalysisCommand { get; set; }

    #endregion

    #region Properties

    public IImprovedMvxViewModel BasicTemplate
    {
        get { return _basicTemplate; }
        set
        {
            SetProperty(ref _basicTemplate, value);
        }
    }

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

    public async Task OpenCombatAnalysisAsync()
    {
        await _mvvmNavigation.Navigate<CombatLogInformationViewModel>();
    }

    public void AuthUpdate(bool isAuth)
    {
        ChatIsEnabled = isAuth;
    }
}
