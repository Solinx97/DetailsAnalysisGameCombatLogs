using CombatAnalysis.Core.Enums;
using CombatAnalysis.Core.Interfaces.Observers;
using CombatAnalysis.Core.Models.GameLogs;
using CombatAnalysis.Core.ViewModels.Base;
using CombatAnalysis.Core.ViewModels.ViewModelTemplates;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using System.Collections.ObjectModel;

namespace CombatAnalysis.Core.ViewModels;

public class CombatsViewModel : ParentTemplate<List<CombatModel>>, IResponseStatusObserver
{
    private readonly IMvxNavigationService _mvvmNavigation;

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

    public CombatsViewModel(IMvxNavigationService mvvmNavigation)
    {
        _mvvmNavigation = mvvmNavigation;

        ShowDetailsCommand = new MvxAsyncCommand(ShowDetailsAsync);

        Basic.Parent = this;
        Basic.SavedViewModel = this;
        Basic.Handler.BasicPropertyUpdate(nameof(BasicTemplateViewModel.Step), 1);

        var responseStatusObservable = Basic as IResponseStatusObservable;
        responseStatusObservable?.AddObserver(this);

        ResponseStatus = ((BasicTemplateViewModel)Basic).ResponseStatus;
    }

    #region Commands

    public IMvxAsyncCommand ShowDetailsCommand { get; }

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

    public void Update(LoadingStatus status)
    {
        ResponseStatus = status;
    }

    private void GetUniqueDungeonNames(List<CombatModel> combats)
    {
        var uniqueDungenNames = combats.DistinctBy(x => x.DungeonName).Select(x => x.DungeonName).ToList();
        DungeonNames = string.Join(" / ", uniqueDungenNames);
    }
}
