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
    private CombatLogModel _combatLog;
    private string _dungeonName;
    private string _name;
    private int _maxCombats;
    private int _currentCombatNumber;

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

    public CombatLogModel CombatLog
    {
        get { return _combatLog; }
        set
        {
            SetProperty(ref _combatLog, value);

            if (value != null)
            {
                RefreshCommand.CanExecute(true);
            }
        }
    }

    public string DungeonName
    {
        get { return _dungeonName; }
        set
        {
            SetProperty(ref _dungeonName, value);
        }
    }

    public string Name
    {
        get { return _name; }
        set
        {
            SetProperty(ref _name, value);
        }
    }

    public int MaxCombats
    {
        get { return _maxCombats; }
        set
        {
            SetProperty(ref _maxCombats, value);
        }
    }

    public int CurrentCombatNumber
    {
        get { return _currentCombatNumber; }
        set
        {
            SetProperty(ref _currentCombatNumber, value);
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
        MaxCombats = Combats.Count;
        CurrentCombatNumber = 0;

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

        BasicTemplate.Handler.PropertyUpdate<BasicTemplateViewModel>(BasicTemplate, nameof(BasicTemplateViewModel.SelectedCombat), SelectedCombat);

        Task.Run(async () => await _mvvmNavigation.Navigate<DetailsSpecificalCombatViewModel, CombatModel>(SelectedCombat));
    }

    public async Task RepeatSaveCombatDataDetailsAsync()
    {
        BasicTemplate.Handler.PropertyUpdate<BasicTemplateViewModel>(BasicTemplate, nameof(BasicTemplateViewModel.ResponseStatus), LoadingStatus.Pending);

        var combatsForUploadAgain = Combats.Where(combat => !combat.IsReady).ToList();

        var combatsAreUploaded = await _combatParserAPIService.SaveAsync(combatsForUploadAgain, CombatLog, _logType, CombatUploaded);
        if (combatsAreUploaded == null)
        {
            BasicTemplate.Handler.PropertyUpdate<BasicTemplateViewModel>(BasicTemplate, nameof(BasicTemplateViewModel.ResponseStatus), LoadingStatus.Failed);
            return;
        }

        var responseStatus = combatsAreUploaded.Any(uploaded => !uploaded) ? LoadingStatus.Failed : LoadingStatus.Successful;
        BasicTemplate.Handler.PropertyUpdate<BasicTemplateViewModel>(BasicTemplate, nameof(BasicTemplateViewModel.ResponseStatus), responseStatus);
    }

    public void Update(LoadingStatus status)
    {
        ResponseStatus = status;
    }

    public async Task RefreshAsync()
    {
        if (CombatLog == null || CombatLog.Id == 0)
        {
            return;
        }

        var loadedCombats = await _combatParserAPIService.LoadCombatsAsync(CombatLog.Id);
        if (loadedCombats == null)
        {
            BasicTemplate.Handler.PropertyUpdate<BasicTemplateViewModel>(BasicTemplate, nameof(BasicTemplateViewModel.ResponseStatus), LoadingStatus.Failed);

            return;
        }

        foreach (var item in Combats)
        {
            var selectUploadedCombat = loadedCombats.FirstOrDefault(x => x.LocallyNumber == item.LocallyNumber);
            if (selectUploadedCombat == null)
            {
                continue;
            }

            item.IsReady = selectUploadedCombat.IsReady;
            Combats = new ObservableCollection<CombatModel>(Combats);
        }
    }

    private void CombatUploaded(int number, string dungeonName, string name)
    {
        CurrentCombatNumber = number;
        DungeonName = dungeonName;
        Name = name;
    }
}
