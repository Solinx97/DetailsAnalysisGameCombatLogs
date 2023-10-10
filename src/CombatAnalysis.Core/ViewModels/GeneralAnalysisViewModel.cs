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
using System.Linq;

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

    public GeneralAnalysisViewModel(IMvxNavigationService mvvmNavigation, IHttpClientHelper httpClient, ILogger logger, IMemoryCache memoryCache)
    {
        _mvvmNavigation = mvvmNavigation;

        _combatParserAPIService = new CombatParserAPIService(httpClient, logger, memoryCache);

        RepeatSaveCommand = new MvxAsyncCommand(RepeatSaveCombatDataDetailsAsync);
        RefreshCommand = new MvxAsyncCommand(RefreshAsync);

        LastCombatInfromationStep = new MvxCommand(LastStep);
        NextCombatInfromationStep = new MvxCommand(NextStep);

        BasicTemplate.Parent = this;
        BasicTemplate.SavedViewModel = this;
        BasicTemplate.Handler.PropertyUpdate<BasicTemplateViewModel>(BasicTemplate, nameof(BasicTemplateViewModel.Step), 1);

        var responseStatusObservable = (IResponseStatusObservable)BasicTemplate;
        responseStatusObservable.AddObserver(this);
    }

    #region Commands

    public IMvxAsyncCommand RepeatSaveCommand { get; set; }

    public IMvxAsyncCommand RefreshCommand { get; set; }

    public IMvxCommand LastCombatInfromationStep { get; set; }

    public IMvxCommand NextCombatInfromationStep { get; set; }

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

    private void GetAverageInformationPerSecond(List<CombatModel> combats)
    {
        var averageDPS = new List<double>();
        var averageHPS = new List<double>();
        var averageRPS = new List<double>();
        var averageDTPS = new List<double>();

        foreach (var combat in combats)
        {
            GetCombatAverageInformation(combat);

            var averageCombatPlayerDPS = combat.Players.Average(x => x.DamageDonePerSecond);
            averageDPS.Add(averageCombatPlayerDPS);

            var averageCombatPlayerHPS = combat.Players.Average(x => x.HealDonePerSecond);
            averageHPS.Add(averageCombatPlayerHPS);

            var averageCombatPlayerRPS = combat.Players.Average(x => x.EnergyRecoveryPerSecond);
            averageRPS.Add(averageCombatPlayerRPS);

            var averageCombatPlayerDTPS = combat.Players.Average(x => x.DamageTakenPerSecond);
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
            var maxCombatPlayerDPS = combat.Players.Max(x => x.DamageDonePerSecond);
            maxDPS.Add(maxCombatPlayerDPS);

            var maxCombatPlayerHPS = combat.Players.Max(x => x.HealDonePerSecond);
            maxHPS.Add(maxCombatPlayerHPS);

            var maxCombatPlayerRPS = combat.Players.Max(x => x.EnergyRecoveryPerSecond);
            maxRPS.Add(maxCombatPlayerRPS);

            var maxCombatPlayerDTPS = combat.Players.Max(x => x.DamageTakenPerSecond);
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
            var averageCombatPlayerDamage = combat.Players.Average(x => x.DamageDone);
            averageDamage.Add(averageCombatPlayerDamage);

            var averageCombatPlayerHeal = combat.Players.Average(x => x.HealDone);
            averageHeal.Add(averageCombatPlayerHeal);

            var averageCombatPlayerResources = combat.Players.Average(x => x.EnergyRecovery);
            averageResources.Add(averageCombatPlayerResources);

            var averageCombatPlayerDamageTaken = combat.Players.Average(x => x.DamageTaken);
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

            var maxCombatPlayerDamage = combat.Players.Max(x => x.DamageDone);
            maxDamage.Add(maxCombatPlayerDamage);

            var maxCombatPlayerHeal = combat.Players.Max(x => x.HealDone);
            maxHeal.Add(maxCombatPlayerHeal);

            var maxCombatPlayerResources = combat.Players.Max(x => x.EnergyRecovery);
            maxResources.Add(maxCombatPlayerResources);

            var maxCombatPlayerDamageTaken = combat.Players.Max(x => x.DamageTaken);
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

    private void GetCombatAverageInformation(CombatModel combat)
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
            player.EnergyRecoveryPerSecond = player.EnergyRecovery / duration.TotalSeconds;
            player.DamageTakenPerSecond = player.DamageTaken / duration.TotalSeconds;
        }
    }
}
