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

public class CombatsViewModel : ParentTemplate<Tuple<List<CombatModel>, LogType>>, IResponseStatusObserver
{
    private readonly IMvxNavigationService _mvvmNavigation;
    private readonly CombatParserAPIService _combatParserAPIService;

    private ObservableCollection<CombatModel> _combats;
    private CombatModel _selectedCombat;
    private int _selectedCombatIndex = -1;
    private LoadingStatus _status;
    private LogType _logType;
    private string _dungeonName;
    private string _name;
    private int _maxCombats;
    private int _currentCombatNumber;
    private double _averageDamagePerSecond;
    private double _averageHealPerSecond;
    private double _averageResourcesPerSecond;
    private double _averageDamageTakenPerSecond;
    private double _maxDamagePerSecond;
    private double _maxHealPerSecond;
    private double _maxResourcesPerSecond;
    private double _maxDamageTakenPerSecond;
    private double _averageDamage;
    private double _averageHeal;
    private double _averageResources;
    private double _averageDamageTaken;
    private double _maxDamage;
    private double _maxHeal;
    private double _maxResources;
    private double _maxDamageTaken;
    private double _indexOfDeath;
    private int _combatInformationStep;
    private int _maxCombatInformationStepIndex = 4;
    private bool _showAverageInformation;
    private string _dungeonNames;

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

        RepeatSaveCommand = new MvxAsyncCommand(RepeatSaveCombatDataDetailsAsync);
        CancelCommand = new MvxCommand(UploadingCancel);
        RefreshCommand = new MvxAsyncCommand(RefreshAsync);
        ShowDetailsCommand = new MvxAsyncCommand(ShowDetailsAsync);
        SortCommand = new MvxCommand<int>(CombatsSort);

        LastCombatInfromationStep = new MvxCommand(LastStep);
        NextCombatInfromationStep = new MvxCommand(NextStep);

        BasicTemplate.Parent = this;
        BasicTemplate.SavedViewModel = this;
        BasicTemplate.Handler.PropertyUpdate<BasicTemplateViewModel>(BasicTemplate, nameof(BasicTemplateViewModel.Step), 1);

        var responseStatusObservable = (IResponseStatusObservable)BasicTemplate;
        responseStatusObservable.AddObserver(this);

        ResponseStatus = ((BasicTemplateViewModel)BasicTemplate).ResponseStatus;
        CurrentCombatNumber = ((BasicTemplateViewModel)BasicTemplate).UploadedCombatsCount;
    }

    public Dictionary<string, List<string>> PetsId { get; set; }

    #region Commands

    public IMvxAsyncCommand RepeatSaveCommand { get; set; }

    public IMvxCommand CancelCommand { get; set; }

    public IMvxAsyncCommand RefreshCommand { get; set; }

    public IMvxCommand LastCombatInfromationStep { get; set; }

    public IMvxCommand NextCombatInfromationStep { get; set; }

    public IMvxAsyncCommand ShowDetailsCommand { get; set; }

    public IMvxCommand SortCommand { get; set; }

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

    public LoadingStatus ResponseStatus
    {
        get { return _status; }
        set
        {
            SetProperty(ref _status, value);
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

    public double AverageDamagePerSecond
    {
        get { return _averageDamagePerSecond; }
        set
        {
            SetProperty(ref _averageDamagePerSecond, value);
        }
    }

    public double AverageHealPerSecond
    {
        get { return _averageHealPerSecond; }
        set
        {
            SetProperty(ref _averageHealPerSecond, value);
        }
    }

    public double AverageResourcesPerSecond
    {
        get { return _averageResourcesPerSecond; }
        set
        {
            SetProperty(ref _averageResourcesPerSecond, value);
        }
    }

    public double AverageDamageTakenPerSecond
    {
        get { return _averageDamageTakenPerSecond; }
        set
        {
            SetProperty(ref _averageDamageTakenPerSecond, value);
        }
    }

    public double MaxDamagePerSecond
    {
        get { return _maxDamagePerSecond; }
        set
        {
            SetProperty(ref _maxDamagePerSecond, value);
        }
    }

    public double MaxHealPerSecond
    {
        get { return _maxHealPerSecond; }
        set
        {
            SetProperty(ref _maxHealPerSecond, value);
        }
    }

    public double MaxResourcesPerSecond
    {
        get { return _maxResourcesPerSecond; }
        set
        {
            SetProperty(ref _maxResourcesPerSecond, value);
        }
    }

    public double MaxDamageTakenPerSecond
    {
        get { return _maxDamageTakenPerSecond; }
        set
        {
            SetProperty(ref _maxDamageTakenPerSecond, value);
        }
    }

    public double AverageDamage
    {
        get { return _averageDamage; }
        set
        {
            SetProperty(ref _averageDamage, value);
        }
    }

    public double AverageHeal
    {
        get { return _averageHeal; }
        set
        {
            SetProperty(ref _averageHeal, value);
        }
    }

    public double AverageResources
    {
        get { return _averageResources; }
        set
        {
            SetProperty(ref _averageResources, value);
        }
    }

    public double AverageDamageTaken
    {
        get { return _averageDamageTaken; }
        set
        {
            SetProperty(ref _averageDamageTaken, value);
        }
    }

    public double MaxDamage
    {
        get { return _maxDamage; }
        set
        {
            SetProperty(ref _maxDamage, value); ;
        }
    }

    public double MaxHeal
    {
        get { return _maxHeal; }
        set
        {
            SetProperty(ref _maxHeal, value);
        }
    }

    public double MaxResources
    {
        get { return _maxResources; }
        set
        {
            SetProperty(ref _maxResources, value);
        }
    }

    public double IndexOfDeath
    {
        get { return _indexOfDeath; }
        set
        {
            SetProperty(ref _indexOfDeath, value);
        }
    }

    public double MaxDamageTaken
    {
        get { return _maxDamageTaken; }
        set
        {
            SetProperty(ref _maxDamageTaken, value);
        }
    }

    public int CombatInformationStep
    {
        get { return _combatInformationStep; }
        set
        {
            SetProperty(ref _combatInformationStep, value);
        }
    }

    public bool ShowAverageInformation
    {
        get { return _showAverageInformation; }
        set
        {
            SetProperty(ref _showAverageInformation, value);
        }
    }

    public string DungeonNames
    {
        get { return _dungeonNames; }
        set
        {
            SetProperty(ref _dungeonNames, value);
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

    protected override void ChildPrepare(Tuple<List<CombatModel>, LogType> parameter)
    {
        if (parameter == null)
        {
            return;
        }

        Combats = new ObservableCollection<CombatModel>(parameter.Item1);
        MaxCombats = Combats.Count;

        _logType = parameter.Item2;

        GetUniqueDungeonNames(parameter.Item1);

        GetAverageInformationPerSecond(parameter.Item1);
        GetMaxInformationPerSecond(parameter.Item1);
        GetAverageInformation(parameter.Item1);
        GetMaxInformation(parameter.Item1);
        GetDeathInformation(parameter.Item1);
    }

    public void NextStep()
    {
        if (CombatInformationStep + 1 > _maxCombatInformationStepIndex)
        {
            CombatInformationStep = 0;
        }
        else
        {
            CombatInformationStep++;
        }
    }

    public void LastStep()
    {
        if (CombatInformationStep - 1 < 0)
        {
            CombatInformationStep = _maxCombatInformationStepIndex;
        }
        else
        {
            CombatInformationStep--;
        }
    }

    public override void ViewDestroy(bool viewFinishing = true)
    {
        var responseStatusObservable = (IResponseStatusObservable)BasicTemplate;
        responseStatusObservable.RemoveObserver(this);

        Combats.Clear();
        MaxCombats = 0;

        base.ViewDestroy(viewFinishing);
    }

    public async Task ShowDetailsAsync()
    {
        if (SelectedCombat == null)
        {
            return;
        }

        BasicTemplate.Handler.PropertyUpdate<BasicTemplateViewModel>(BasicTemplate, nameof(BasicTemplateViewModel.SelectedCombat), SelectedCombat);

        await _mvvmNavigation.Navigate<CombatPlayersViewModel, CombatModel>(SelectedCombat);
    }

    public async Task RepeatSaveCombatDataDetailsAsync()
    {
        var token = ((BasicTemplateViewModel)BasicTemplate).RequestCancelationToken();

        CurrentCombatNumber = 0;

        BasicTemplate.Handler.PropertyUpdate<BasicTemplateViewModel>(BasicTemplate, nameof(BasicTemplateViewModel.ResponseStatus), LoadingStatus.Pending);
        BasicTemplate.Handler.PropertyUpdate<BasicTemplateViewModel>(BasicTemplate, nameof(BasicTemplateViewModel.UploadedCombatsCount), 0);

        var combatsForUploadAgain = Combats.Where(combat => !combat.IsReady).ToList();

        var combotLogToRepeat = ((BasicTemplateViewModel)BasicTemplate).CombatLog;
        var combatsAreUploaded = await _combatParserAPIService.SaveAsync(combatsForUploadAgain, combotLogToRepeat, _logType, CombatUploaded, token);
        if (!combatsAreUploaded)
        {
            BasicTemplate.Handler.PropertyUpdate<BasicTemplateViewModel>(BasicTemplate, nameof(BasicTemplateViewModel.ResponseStatus), LoadingStatus.Failed);

            return;
        }

        var responseStatus = LoadingStatus.Successful;
        BasicTemplate.Handler.PropertyUpdate<BasicTemplateViewModel>(BasicTemplate, nameof(BasicTemplateViewModel.ResponseStatus), responseStatus);
    }

    public void Update(LoadingStatus status)
    {
        ResponseStatus = status;
    }

    public async Task RefreshAsync()
    {
        var combatLog = ((BasicTemplateViewModel)BasicTemplate).CombatLog;
        if (combatLog == null || combatLog.Id == 0)
        {
            return;
        }

        _combatParserAPIService.SetUpPort();
        var loadedCombats = await _combatParserAPIService.LoadCombatsAsync(combatLog.Id);
        if (loadedCombats == null || !loadedCombats.Any())
        {
            return;
        }

        foreach (var item in loadedCombats)
        {
            var players = await _combatParserAPIService.LoadCombatPlayersAsync(item.Id);
            item.Players = players.ToList();
        }

        Combats = new ObservableCollection<CombatModel>(loadedCombats);
    }

    public void CombatsSort(int sortNumber)
    {
        var sortedCollection = Combats.ToList();
        switch (sortNumber)
        {
            case 0:
                sortedCollection = SortedByName == 0
                    ? Combats.OrderByDescending(x => x.Name).ToList()
                    : Combats.OrderBy(x => x.Name).ToList();
                SortedByName = SortedByName == 0 ? 1 : 0;

                SortedByDamageDone = -1;
                SortedByHealDone = -1;
                SortedByDamageTaken = -1;
                SortedByResources = -1;
                SortedByDeaths = -1;
                break;
            case 1:
                sortedCollection = SortedByDamageDone == 0
                    ? Combats.OrderByDescending(x => x.DamageDone).ToList()
                    : Combats.OrderBy(x => x.DamageDone).ToList();
                SortedByDamageDone = SortedByDamageDone == 0 ? 1 : 0;

                SortedByName = -1;
                SortedByHealDone = -1;
                SortedByDamageTaken = -1;
                SortedByResources = -1;
                SortedByDeaths = -1;
                break;
            case 2:
                sortedCollection = SortedByHealDone == 0
                    ? Combats.OrderByDescending(x => x.HealDone).ToList()
                    : Combats.OrderBy(x => x.HealDone).ToList();
                SortedByHealDone = SortedByHealDone == 0 ? 1 : 0;

                SortedByName = -1;
                SortedByDamageDone = -1;
                SortedByDamageTaken = -1;
                SortedByResources = -1;
                SortedByDeaths = -1;
                break;
            case 3:
                sortedCollection = SortedByDamageTaken == 0
                    ? Combats.OrderByDescending(x => x.DamageTaken).ToList()
                    : Combats.OrderBy(x => x.DamageTaken).ToList();
                SortedByDamageTaken = SortedByDamageTaken == 0 ? 1 : 0;

                SortedByName = SortedByName == -1 ? 0 : 1;
                SortedByDamageDone = -1;
                SortedByHealDone = -1;
                SortedByResources = -1;
                SortedByDeaths = -1;
                break;
            case 4:
                sortedCollection = SortedByResources == 0
                    ? Combats.OrderByDescending(x => x.EnergyRecovery).ToList()
                    : Combats.OrderBy(x => x.EnergyRecovery).ToList();
                SortedByResources = SortedByResources == 0 ? 1 : 0;

                SortedByName = -1;
                SortedByDamageDone = -1;
                SortedByHealDone = -1;
                SortedByDamageTaken = -1;
                SortedByDeaths = -1;
                break;
            case 5:
                sortedCollection = SortedByDeaths == 0
                    ? Combats.OrderByDescending(x => x.DeathNumber).ToList()
                    : Combats.OrderBy(x => x.DeathNumber).ToList();
                SortedByDeaths = SortedByDeaths == 0 ? 1 : 0;

                SortedByName = -1;
                SortedByDamageDone = -1;
                SortedByHealDone = -1;
                SortedByDamageTaken = -1;
                SortedByResources = -1;
                break;
        }

        Combats = new ObservableCollection<CombatModel>(sortedCollection);
    }

    public void UploadingCancel()
    {
        ((BasicTemplateViewModel)BasicTemplate).RequestCancel();
    }

    private void GetUniqueDungeonNames(List<CombatModel> combats)
    {
        var uniqueDungenNames = combats.DistinctBy(x => x.DungeonName).Select(x => x.DungeonName).ToList();
        DungeonNames = string.Join(" / ", uniqueDungenNames);
    }

    private void CombatUploaded(int number, string dungeonName, string name)
    {
        CurrentCombatNumber = number;
        DungeonName = dungeonName;
        Name = name;
    }

    private void GetAverageInformationPerSecond(List<CombatModel> combats)
    {
        var averageDPS = new List<double>();
        var averageHPS = new List<double>();
        var averageRPS = new List<double>();
        var averageDTPS = new List<double>();

        foreach (var combat in combats)
        {
            GetCombatAverageInformation(combat);

            var averageCombatPlayerDPS = combat.Players.Any() ? combat.Players.Average(x => x.DamageDonePerSecond) : 0;
            averageDPS.Add(averageCombatPlayerDPS);

            var averageCombatPlayerHPS = combat.Players.Any() ? combat.Players.Average(x => x.HealDonePerSecond) : 0;
            averageHPS.Add(averageCombatPlayerHPS);

            var averageCombatPlayerRPS = combat.Players.Any() ? combat.Players.Average(x => x.ResourcesRecoveryPerSecond) : 0;
            averageRPS.Add(averageCombatPlayerRPS);

            var averageCombatPlayerDTPS = combat.Players.Any() ? combat.Players.Average(x => x.DamageTakenPerSecond) : 0;
            averageDTPS.Add(averageCombatPlayerDTPS);
        }

        AverageDamagePerSecond = averageDPS.Average();
        AverageHealPerSecond = averageHPS.Average();
        AverageResourcesPerSecond = averageRPS.Average();
        AverageDamageTakenPerSecond = averageDTPS.Average();
    }

    private void GetMaxInformationPerSecond(List<CombatModel> combats)

    {
        var maxDPS = new List<double>();
        var maxHPS = new List<double>();
        var maxRPS = new List<double>();
        var maxDTPS = new List<double>();

        foreach (var combat in combats)
        {
            var maxCombatPlayerDPS = combat.Players.Any() ? combat.Players.Max(x => x.DamageDonePerSecond) : 0;
            maxDPS.Add(maxCombatPlayerDPS);

            var maxCombatPlayerHPS = combat.Players.Any() ? combat.Players.Max(x => x.HealDonePerSecond) : 0;
            maxHPS.Add(maxCombatPlayerHPS);

            var maxCombatPlayerRPS = combat.Players.Any() ? combat.Players.Max(x => x.ResourcesRecoveryPerSecond) : 0;
            maxRPS.Add(maxCombatPlayerRPS);

            var maxCombatPlayerDTPS = combat.Players.Any() ? combat.Players.Max(x => x.DamageTakenPerSecond) : 0;
            maxDTPS.Add(maxCombatPlayerDTPS);
        }

        MaxDamagePerSecond = maxDPS.Max();
        MaxHealPerSecond = maxHPS.Max();
        MaxResourcesPerSecond = maxRPS.Max();
        MaxDamageTakenPerSecond = maxDTPS.Max();
    }

    private void GetAverageInformation(List<CombatModel> combats)
    {
        var averageDamage = new List<double>();
        var averageHeal = new List<double>();
        var averageResources = new List<double>();
        var averageDamageTaken = new List<double>();

        foreach (var combat in combats)
        {
            var averageCombatPlayerDamage = combat.Players.Any() ? combat.Players.Average(x => x.DamageDone) : 0;
            averageDamage.Add(averageCombatPlayerDamage);

            var averageCombatPlayerHeal = combat.Players.Any() ? combat.Players.Average(x => x.HealDone) : 0;
            averageHeal.Add(averageCombatPlayerHeal);

            var averageCombatPlayerResources = combat.Players.Any() ? combat.Players.Average(x => x.ResourcesRecovery) : 0;
            averageResources.Add(averageCombatPlayerResources);

            var averageCombatPlayerDamageTaken = combat.Players.Any() ? combat.Players.Average(x => x.DamageTaken) : 0;
            averageDamageTaken.Add(averageCombatPlayerDamageTaken);
        }

        AverageDamage = averageDamage.Average();
        AverageHeal = averageHeal.Average();
        AverageResources = averageResources.Average();
        AverageDamageTaken = averageDamageTaken.Average();
    }

    private void GetMaxInformation(List<CombatModel> combats)
    {
        var maxDamage = new List<double>();
        var maxHeal = new List<double>();
        var maxResources = new List<double>();
        var maxDamageTaken = new List<double>();

        foreach (var combat in combats)
        {
            GetCombatAverageInformation(combat);

            var maxCombatPlayerDamage = combat.Players.Any() ? combat.Players.Max(x => x.DamageDone) : 0;
            maxDamage.Add(maxCombatPlayerDamage);

            var maxCombatPlayerHeal = combat.Players.Any() ? combat.Players.Max(x => x.HealDone) : 0;
            maxHeal.Add(maxCombatPlayerHeal);

            var maxCombatPlayerResources = combat.Players.Any() ? combat.Players.Max(x => x.ResourcesRecovery) : 0;
            maxResources.Add(maxCombatPlayerResources);

            var maxCombatPlayerDamageTaken = combat.Players.Any() ? combat.Players.Max(x => x.DamageTaken) : 0;
            maxDamageTaken.Add(maxCombatPlayerDamageTaken);
        }

        MaxDamage = maxDamage.Max();
        MaxHeal = maxHeal.Max();
        MaxResources = maxResources.Max();
        MaxDamageTaken = maxDamageTaken.Max();
    }

    private void GetDeathInformation(List<CombatModel> combats)
    {
        IndexOfDeath = combats.Average(x => x.DeathNumber);
    }

    private static void GetCombatAverageInformation(CombatModel combat)
    {
        TimeSpan duration;
        if (!TimeSpan.TryParse(combat.Duration, out duration))
        {
            duration = TimeSpan.Zero;

            return;
        }

        foreach (var player in combat.Players)
        {
            player.DamageDonePerSecond = player.DamageDone / duration.TotalSeconds;
            player.HealDonePerSecond = player.HealDone / duration.TotalSeconds;
            player.ResourcesRecoveryPerSecond = player.ResourcesRecovery / duration.TotalSeconds;
            player.DamageTakenPerSecond = player.DamageTaken / duration.TotalSeconds;
        }
    }
}
