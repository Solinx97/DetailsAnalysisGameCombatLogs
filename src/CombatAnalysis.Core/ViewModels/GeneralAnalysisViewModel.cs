using CombatAnalysis.Core.Enums;
using CombatAnalysis.Core.Interfaces;
using CombatAnalysis.Core.Interfaces.Observers;
using CombatAnalysis.Core.Models;
using CombatAnalysis.Core.Services;
using CombatAnalysis.Core.ViewModels.Base;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using System.Collections.ObjectModel;

namespace CombatAnalysis.Core.ViewModels;

public class GeneralAnalysisViewModel : ParentTemplate<Tuple<List<CombatModel>, LogType>>, IResponseStatusObserver
{
    private readonly IMvxNavigationService _mvvmNavigation;
    private readonly CombatParserAPIService _combatParserAPIService;

    private ObservableCollection<CombatModel> _combats;
    private CombatModel _selectedCombat;
    private LoadingStatus _status;
    private LogType _logType;
    private int _combatLogId;
    private bool _combatIsNotLoaded = false;

    public GeneralAnalysisViewModel(IMvxNavigationService mvvmNavigation, IHttpClientHelper httpClient, ILogger logger, IMemoryCache memoryCache)
    {
        _mvvmNavigation = mvvmNavigation;

        _combatParserAPIService = new CombatParserAPIService(httpClient, logger, memoryCache);

        RepeatSaveCommand = new MvxAsyncCommand(RepeatSaveCombatDataDetailsAsync);
        RefreshCommand = new MvxAsyncCommand(RefreshAsync);

        BasicTemplate.Parent = this;
        BasicTemplate.SavedViewModel = this;
        BasicTemplate.Handler.PropertyUpdate<BasicTemplateViewModel>(BasicTemplate, nameof(BasicTemplateViewModel.Step), 1);

        var responseStatusObservable = (IResponseStatusObservable)BasicTemplate;
        responseStatusObservable.AddObserver(this);
    }

    #region Commands

    public IMvxAsyncCommand RepeatSaveCommand { get; set; }

    public IMvxAsyncCommand RefreshCommand { get; set; }

    #endregion

    #region Properties

    public ObservableCollection<CombatModel> Combats
    {
        get { return _combats; }
        set
        {
            SetProperty(ref _combats, value);
        }
    }

    public CombatModel SelectedCombat
    {
        get { return _selectedCombat; }
        set
        {
            SetProperty(ref _selectedCombat, value);

            ShowDetails();
        }
    }

    public LoadingStatus ResponseStatus
    {
        get { return _status; }
        set
        {
            SetProperty(ref _status, value);
        }
    }

    public int CombatLogId
    {
        get { return _combatLogId; }
        set
        {
            SetProperty(ref _combatLogId, value);

            if (value > 0)
            {
                RefreshCommand.CanExecute(true);
            }
        }
    }

    public bool CombatIsNotLoaded
    {
        get { return _combatIsNotLoaded; }
        set
        {
            SetProperty(ref _combatIsNotLoaded, value);
        }
    }

    #endregion

    protected override void ChildPrepare(Tuple<List<CombatModel>, LogType> parameter)
    {
        if (parameter == null)
        {
            return;
        }

        Combats = new ObservableCollection<CombatModel>(parameter.Item1);
        _logType = parameter.Item2;
    }

    public override void ViewDestroy(bool viewFinishing = true)
    {
        ((BasicTemplateViewModel)BasicTemplate).Combats = Combats.ToList();

        base.ViewDestroy(viewFinishing);
    }

    public void ShowDetails()
    {
        if (SelectedCombat == null)
        {
            return;
        }

        if (!SelectedCombat.IsReady && SelectedCombat.Id > 0)
        {
            CombatIsNotLoaded = true;
            return;
        }

        BasicTemplate.Handler.PropertyUpdate<BasicTemplateViewModel>(BasicTemplate, nameof(BasicTemplateViewModel.SelectedCombat), SelectedCombat);

        Task.Run(async () => await _mvvmNavigation.Navigate<DetailsSpecificalCombatViewModel, CombatModel>(SelectedCombat));
    }

    public async Task RepeatSaveCombatDataDetailsAsync()
    {
        BasicTemplate.Handler.PropertyUpdate<BasicTemplateViewModel>(BasicTemplate, nameof(BasicTemplateViewModel.ResponseStatus), LoadingStatus.Pending);

        var responseStatus = await _combatParserAPIService.SaveAsync(Combats.ToList(), _logType) > 0 ? LoadingStatus.Successful : LoadingStatus.Failed;
        BasicTemplate.Handler.PropertyUpdate<BasicTemplateViewModel>(BasicTemplate, nameof(BasicTemplateViewModel.ResponseStatus), responseStatus);
    }

    public void Update(LoadingStatus status)
    {
        ResponseStatus = status;
    }

    public async Task RefreshAsync()
    {
        CombatIsNotLoaded = false;

        if (CombatLogId == 0)
        {
            return;
        }

        var loadedCombats = await _combatParserAPIService.LoadCombatsAsync(CombatLogId);

        foreach (var item in loadedCombats)
        {
            var players = await _combatParserAPIService.LoadCombatPlayersAsync(item.Id);
            item.Players = players.ToList();
        }

        Combats = new ObservableCollection<CombatModel>(loadedCombats);
    }
}
