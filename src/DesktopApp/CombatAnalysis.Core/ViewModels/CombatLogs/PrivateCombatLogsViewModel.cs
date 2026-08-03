using CombatAnalysis.Core.Enums;
using CombatAnalysis.Core.Interfaces;
using CombatAnalysis.Core.Models.GameLogs;
using CombatAnalysis.Core.Models.User;
using CombatAnalysis.Core.ViewModels.Base;
using CombatAnalysis.Core.ViewModels.ViewModelTemplates;
using Microsoft.Extensions.Caching.Memory;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using System.Collections.ObjectModel;

namespace CombatAnalysis.Core.ViewModels.CombatLogs;

public class PrivateCombatLogsViewModel : ParentTemplate
{
    private readonly IMvxNavigationService _mvvmNavigation;
    private readonly ICombatParserAPIService _combatParserAPIService;
    private readonly IMemoryCache _memoryCache;

    private ObservableCollection<CombatLogModel> _combatLogs = [];
    
    private int _combatListSelectedIndex;
    private bool _isAuth;
    private LoadingStatus _combatLogLoadingStatus;
    private bool _removingInProgress;
    private bool _uploadingLogs;

    public PrivateCombatLogsViewModel(IMvxNavigationService mvvmNavigation, ICombatParserAPIService combatParserAPIService, IMemoryCache memoryCache)
    {
        _mvvmNavigation = mvvmNavigation;
        _combatParserAPIService = combatParserAPIService;
        _memoryCache = memoryCache;

        LoadSelectedCombatLogCommand = new MvxAsyncCommand(() => LoadSelectedCombatLogAsync(CombatLogsForTargetUser));
        ReloadCombatLogsCommand = new MvxAsyncCommand(LoadCombatLogsAsync);
        DeleteCombatCommand = new MvxAsyncCommand(DeleteAsync);
    }

    #region Commands

    public IMvxAsyncCommand LoadSelectedCombatLogCommand { get; private set; }

    public IMvxAsyncCommand ReloadCombatLogsCommand { get; private set; }

    public IMvxAsyncCommand DeleteCombatCommand { get; private set; }

    #endregion

    public AppUserModel? User { get; private set; }

    #region View model properties

    public bool UploadingLogs
    {
        get { return _uploadingLogs; }
        set
        {
            SetProperty(ref _uploadingLogs, value);
        }
    }

    public ObservableCollection<CombatLogModel> CombatLogsForTargetUser
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

    public bool RemovingInProgress
    {
        get { return _removingInProgress; }
        set
        {
            SetProperty(ref _removingInProgress, value);
        }
    }

    #endregion

    #region Ovveride methods

    public override async Task Initialize()
    {
        GetUser();

        var token = ((BasicTemplateViewModel)Basic).RequestCancelationToken();
        await LoadCombatLogsAsync(token);

        await base.Initialize();
    }

    #endregion
    
    private void GetUser()
    {
        var user = _memoryCache.Get<AppUserModel>(nameof(MemoryCacheValue.User));
        User = user;
    }

    private async Task LoadCombatLogsAsync(CancellationToken cancellationToken)
    {
        CombatLogLoadingStatus = LoadingStatus.Pending;

        var combatLogs = await _combatParserAPIService.LoadCombatLogsAsync((int)LogType.Private, User?.Id, cancellationToken);
        if (combatLogs == null)
        {
            CombatLogLoadingStatus = LoadingStatus.Failed;

            return;
        }

        var privateLogs = combatLogs.Where(x => x.LogType == (int)LogType.Private).ToList();
        CombatLogsForTargetUser = new ObservableCollection<CombatLogModel>(combatLogs);

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

    private async Task DeleteAsync()
    {
        RemovingInProgress = true;

        var token = ((BasicTemplateViewModel)Basic).RequestCancelationToken();
        var selectedCombatLogByUser = _combatLogs.FirstOrDefault(x => x.Id == CombatLogsForTargetUser[CombatListSelectedIndex].Id);
        if (selectedCombatLogByUser != null)
        {
            await _combatParserAPIService.DeleteCombatLogByUserAsync(selectedCombatLogByUser.Id, token);
        }

        await LoadCombatLogsAsync(token);

        RemovingInProgress = false;
    }
}
