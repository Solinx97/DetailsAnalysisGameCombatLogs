using CombatAnalysis.Core.Enums;
using CombatAnalysis.Core.Interfaces;
using CombatAnalysis.Core.Interfaces.Observers;
using CombatAnalysis.Core.Models.GameLogs;
using CombatAnalysis.Core.Services;
using CombatAnalysis.Core.ViewModels.Base;
using CombatAnalysis.Core.ViewModels.ViewModelTemplates;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using System.Collections.ObjectModel;

namespace CombatAnalysis.Core.ViewModels;

public class CombatsViewModel : ParentTemplate<List<CombatModel>>, IResponseStatusObserver
{
    private readonly IMvxNavigationService _mvvmNavigation;
    private readonly CombatParserAPIService _combatParserAPIService;

    private ObservableCollection<CombatModel>? _uniqueCombats;
    private ObservableCollection<CombatModel>? _allCombats;
    private CombatModel? _selectedCombat;
    private int _combatsNumber;
    private int _selectedCombatIndex = -1;
    private int _selectedUniqueCombatNumber = -1;
    private string? _dungeonName;
    private string? _dungeonNames;
    private string? _name;
    private LoadingStatus _status;
    private int _currentCombatNumber;

    private int _sortedByName = -1;
    private int _sortedByDamageDone = -1;
    private int _sortedByHealDone = -1;
    private int _sortedByDamageTaken = -1;
    private int _sortedByResources = -1;
    private int _sortedByDeaths = -1;

    public CombatsViewModel(IMvxNavigationService mvvmNavigation, IHttpClientHelper httpClient, ILogger logger, IMemoryCache memoryCache)
    {
        _mvvmNavigation = mvvmNavigation;

        _combatParserAPIService = new CombatParserAPIService(httpClient, logger, memoryCache);

        RepeatSaveCommand = new MvxAsyncCommand(SaveCombatsAsync);
        CancelCommand = new MvxCommand(UploadingCancel);
        ShowDetailsCommand = new MvxAsyncCommand(ShowDetailsAsync);

        Basic.Parent = this;
        Basic.SavedViewModel = this;
        Basic.Handler.BasicPropertyUpdate(nameof(BasicTemplateViewModel.Step), 1);

        var responseStatusObservable = Basic as IResponseStatusObservable;
        responseStatusObservable?.AddObserver(this);

        ResponseStatus = ((BasicTemplateViewModel)Basic).ResponseStatus;
    }

    #region Commands

    public IMvxAsyncCommand RepeatSaveCommand { get; }

    public IMvxCommand CancelCommand { get; }

    public IMvxAsyncCommand ShowDetailsCommand { get; }

    public IMvxCommand CombatSortCommand { get; }

    #endregion

    #region View model properties

    public ObservableCollection<CombatModel>? UniqueCombats
    {
        get { return _uniqueCombats; }
        set
        {
            SetProperty(ref _uniqueCombats, value);
        }
    }

    public int CombatsNumber
    {
        get { return _combatsNumber; }
        set
        {
            SetProperty(ref _combatsNumber, value);
        }
    }

    public CombatModel? SelectedCombat
    {
        get { return _selectedCombat; }
        set
        {
            if (value != null)
            {
                value.Number = SelectedCombatIndex;
            }

            SetProperty(ref _selectedCombat, value);
        }
    }

    public int SelectedCombatIndex
    {
        get { return _selectedCombatIndex; }
        set
        {
            SetProperty(ref _selectedCombatIndex, value);
        }
    }

    public int SelectedUniqueCombatNumber
    {
        get { return _selectedUniqueCombatNumber; }
        set
        {
            SetProperty(ref _selectedUniqueCombatNumber, value);
            if (value > 0 && SelectedCombat != null && _allCombats != null)
            {
                SelectedCombat = _allCombats.Where(c => c.Boss.GameId == SelectedCombat.Boss.GameId).ToArray()[value - 1];
                Task.Run(async () => await ShowDetailsCommand.ExecuteAsync());
            }
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

    public string? DungeonName
    {
        get { return _dungeonName; }
        set
        {
            SetProperty(ref _dungeonName, value);
        }
    }

    public string? DungeonNames
    {
        get { return _dungeonNames; }
        set
        {
            SetProperty(ref _dungeonNames, value);
        }
    }

    public string? Name
    {
        get { return _name; }
        set
        {
            SetProperty(ref _name, value);
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

    #region Sort properties

    public int SortedByName
    {
        get { return _sortedByName; }
        set
        {
            SetProperty(ref _sortedByName, value);
        }
    }

    public int SortedByDamageDone
    {
        get { return _sortedByDamageDone; }
        set
        {
            SetProperty(ref _sortedByDamageDone, value);
        }
    }

    public int SortedByHealDone
    {
        get { return _sortedByHealDone; }
        set
        {
            SetProperty(ref _sortedByHealDone, value);
        }
    }

    public int SortedByDamageTaken
    {
        get { return _sortedByDamageTaken; }
        set
        {
            SetProperty(ref _sortedByDamageTaken, value);
        }
    }

    public int SortedByResources
    {
        get { return _sortedByResources; }
        set
        {
            SetProperty(ref _sortedByResources, value);
        }
    }

    public int SortedByDeaths
    {
        get { return _sortedByDeaths; }
        set
        {
            SetProperty(ref _sortedByDeaths, value);
        }
    }

    #endregion

    public override void Prepare(List<CombatModel> parameter)
    {
        if (parameter == null || parameter.Count == 0)
        {
            return;
        }

        _allCombats = new ObservableCollection<CombatModel>(parameter);
        CombatsNumber = _allCombats.Count;

        var uniqueCombats = _allCombats
            .GroupBy(c => c.Boss.Id)
            .Select(c =>
            {
                var combat = c.Any(x => x.IsWin) ? c.First(x => x.IsWin) : c.Last();
                combat.Items = [];

                var allBossCombats = _allCombats.Where(x => x.Boss.GameId == combat.Boss.GameId).ToArray();
                combat.UniqueCombatCount = c.Count();

                int[] combatNumbers = [.. Enumerable.Range(0, combat.UniqueCombatCount - 1)];
                foreach (var index in combatNumbers)
                {
                    var percentage = index < allBossCombats.Length ? allBossCombats[index].BossHealthPercentage : 0.0;
                    combat.Items.Add(index + 1, percentage);
                }

                return combat;
            })
            .OrderBy(c => c.FinishDate)
            .ToList();
        UniqueCombats = new ObservableCollection<CombatModel>(uniqueCombats);

        GetUniqueDungeonNames(parameter);
    }

    public override async Task Initialize()
    {
        var isCombatLogsMustBeSaved = ((BasicTemplateViewModel)Basic).IsCombatLogsMustSave;
        if (isCombatLogsMustBeSaved)
        {
            await SaveCombatsAsync();
        }

        await base.Initialize();
    }

    public override void ViewDestroy(bool viewFinishing = true)
    {
        var responseStatusObservable = Basic as IResponseStatusObservable;
        responseStatusObservable?.RemoveObserver(this);

        UniqueCombats?.Clear();
        _allCombats?.Clear();

        base.ViewDestroy(viewFinishing);
    }

    private async Task ShowDetailsAsync()
    {
        if (SelectedCombat == null)
        {
            return;
        }

        Basic.Handler.BasicPropertyUpdate(nameof(BasicTemplateViewModel.SelectedCombat), SelectedCombat);

        await _mvvmNavigation.Navigate<CombatPlayersViewModel, CombatModel>(SelectedCombat);
    }

    private async Task SaveCombatsAsync()
    {
        try
        {
            CurrentCombatNumber = 0;

            Basic.Handler.BasicPropertyUpdate(nameof(BasicTemplateViewModel.ResponseStatus), LoadingStatus.Pending);

            var combats = _allCombats == null ? [] : _allCombats.ToList();
            var combatLog = ((BasicTemplateViewModel)Basic).CombatLog;
            if (combatLog == null)
            {
                return;
            }

            await _combatParserAPIService.SaveAsync(combats, combatLog, CombatUploaded, ((BasicTemplateViewModel)Basic).RequestCancelationToken);

            Basic.Handler.BasicPropertyUpdate(nameof(BasicTemplateViewModel.ResponseStatus), LoadingStatus.Successful);
            Basic.Handler.BasicPropertyUpdate(nameof(BasicTemplateViewModel.IsCombatLogsMustSave), false);
        }
        catch (Exception)
        {
            Basic.Handler.BasicPropertyUpdate(nameof(BasicTemplateViewModel.ResponseStatus), LoadingStatus.Failed);
            Basic.Handler.BasicPropertyUpdate(nameof(BasicTemplateViewModel.IsCombatLogsMustSave), false);
        }
    }

    public void Update(LoadingStatus status)
    {
        ResponseStatus = status;
    }

    public void UploadingCancel()
    {
        ((BasicTemplateViewModel)Basic).RequestCancel();
    }

    private void GetUniqueDungeonNames(List<CombatModel> combats)
    {
        var uniqueDungenNames = combats.DistinctBy(x => x.DungeonName).Select(x => x.DungeonName).ToList();
        DungeonNames = string.Join(" / ", uniqueDungenNames);
    }

    private void CombatUploaded(string dungeonName, string name)
    {
        DungeonName = dungeonName;
        Name = name;

        CurrentCombatNumber++;
    }
}
