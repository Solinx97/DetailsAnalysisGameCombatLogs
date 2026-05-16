using CombatAnalysis.Core.Enums;
using CombatAnalysis.Core.Interfaces;
using CombatAnalysis.Core.Models.GameLogs;
using CombatAnalysis.Core.ViewModels.Base;
using CombatAnalysis.Core.ViewModels.ViewModelTemplates;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using System.Collections.ObjectModel;

namespace CombatAnalysis.Core.ViewModels.CombatLogs;

public class PublicCombatLogsViewModel : ParentTemplate
{
    private readonly IMvxNavigationService _mvvmNavigation;
    private readonly ICombatParserAPIService _combatParserAPIService;

    private ObservableCollection<CombatLogModel> _combatLogs = [];
    
    private int _combatListSelectedIndex;
    private bool _isAuth;
    private LoadingStatus _combatLogLoadingStatus;
    private bool _uploadingLogs;

    public PublicCombatLogsViewModel(IMvxNavigationService mvvmNavigation, ICombatParserAPIService combatParserAPIService)
    {
        _mvvmNavigation = mvvmNavigation;
        _combatParserAPIService = combatParserAPIService;

        LoadSelectedCombatLogCommand = new MvxAsyncCommand(() => LoadSelectedCombatLogAsync(CombatLogs));
        ReloadCombatLogsCommand = new MvxAsyncCommand(LoadCombatLogsAsync);

        Basic.Parent = this;
        Basic.Handler.BasicPropertyUpdate(nameof(BasicTemplateViewModel.Step), 0);
        Basic.Handler.BasicPropertyUpdate(nameof(BasicTemplateViewModel.LogPanelStatusIsVisibly), true);
    }

    #region Commands

    public IMvxAsyncCommand LoadSelectedCombatLogCommand { get; private set; }

    public IMvxAsyncCommand ReloadCombatLogsCommand { get; private set; }

    #endregion

    #region View model properties

    public bool UploadingLogs
    {
        get { return _uploadingLogs; }
        set
        {
            SetProperty(ref _uploadingLogs, value);
        }
    }

    public ObservableCollection<CombatLogModel> CombatLogs
    {
        get { return _combatLogs; }
        set
        {
            SetProperty(ref _combatLogs, value);
        }
    }

    public int CombatListSelectedIndex
    {
        get { return _combatListSelectedIndex; }
        set
        {
            SetProperty(ref _combatListSelectedIndex, value);
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

    public LoadingStatus CombatLogLoadingStatus
    {
        get { return _combatLogLoadingStatus; }
        set
        {
            SetProperty(ref _combatLogLoadingStatus, value);
        }
    }

    #endregion

    #region Ovveride methods

    public override async Task Initialize()
    {
        var token = ((BasicTemplateViewModel)Basic).RequestCancelationToken();
        await LoadCombatLogsAsync(token);

        await base.Initialize();
    }

    #endregion

    private async Task LoadCombatLogsAsync(CancellationToken cancellationToken)
    {
        CombatLogLoadingStatus = LoadingStatus.Pending;

        var combatLogsData = await _combatParserAPIService.LoadCombatLogsAsync(cancellationToken);
        if (combatLogsData == null)
        {
            CombatLogLoadingStatus = LoadingStatus.Failed;
            CombatLogs = [];

            return;
        }

        var publicLogs = combatLogsData.Where(x => x.LogType == (int)LogType.Public).ToList();
        CombatLogs = new ObservableCollection<CombatLogModel>(publicLogs);

        CombatLogLoadingStatus = LoadingStatus.Successful;
    }

    private async Task LoadSelectedCombatLogAsync(ObservableCollection<CombatLogModel> combatCollection)
    {
        UploadingLogs = true;

        var combatLog = combatCollection[CombatListSelectedIndex];

        var token = ((BasicTemplateViewModel)Basic).RequestCancelationToken();
        var loadedCombats = await _combatParserAPIService.LoadCombatsAsync(combatLog.Id, token);
        if (!loadedCombats.Any())
        {
            return;
        }

        Basic.Handler.BasicPropertyUpdate(nameof(BasicTemplateViewModel.AllowStep), 1);

        await _mvvmNavigation.Navigate<CombatsViewModel, List<CombatModel>>([.. loadedCombats]);

        Basic.Handler.BasicPropertyUpdate(nameof(BasicTemplateViewModel.CombatLog), combatLog);
        Basic.Handler.BasicPropertyUpdate(nameof(BasicTemplateViewModel.Combats), loadedCombats.ToList());

        UploadingLogs = false;
    }
}
