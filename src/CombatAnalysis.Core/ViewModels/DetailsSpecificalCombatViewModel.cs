using CombatAnalysis.Core.Localizations;
using CombatAnalysis.Core.Models;
using CombatAnalysis.Core.ViewModels.Base;
using MvvmCross.Commands;

namespace CombatAnalysis.Core.ViewModels;

public class DetailsSpecificalCombatViewModel : ParentTemplate<CombatModel>
{
    private CombatModel _combat;
    private List<CombatPlayerModel> _playersCombat;
    private List<CombatPlayerModel> _mainPlayersCombat;
    private CombatPlayerModel _selectedPlayer;
    private int _combatInformationType;
    private List<string> _filterList;
    private int _selectedFilterIndex;
    private int _minDamageDone;
    private int _minHealDone;
    private int _minEnergyRecovery;
    private bool _openEditMinDamageDone;
    private bool _openEditMinHealDone;
    private bool _openEditMinEnergyRecovery;
    private bool _useFilterByMinDamageDone;
    private bool _useFilterByMinHealDone;
    private bool _useFilterByMinEnergyRecovery;
    private int _minDPS;
    private int _minHPS;
    private int _minRPS;
    private bool _openEditMinDPS;
    private bool _openEditMinHPS;
    private bool _openEditMinRPS;
    private bool _useFilterByMinDPS;
    private bool _useFilterByMinHPS;
    private bool _useFilterByMinRPS;
    private double _averageDamage;
    private double _averageHeal;
    private double _averageResources;
    private double _averageDamagePerSecond;
    private double _averageHealPerSecond;
    private double _averageResourcesPerSecond;
    private bool _showSummaryInformation;
    private int _totalDamage;
    private int _totalHeal;
    private int _totalResources;
    private double _totalDamagePerSecond;
    private double _totalHealPerSecond;
    private double _totalResourcesPerSecond;

    public DetailsSpecificalCombatViewModel()
    {
        BasicTemplate.Parent = this;
        BasicTemplate.Handler.PropertyUpdate<BasicTemplateViewModel>(BasicTemplate, nameof(BasicTemplateViewModel.Step), 2);

        SwitchBetweenValuesCommand = new MvxCommand<int>(SwitchBetweenValues);

        OpenEditMinDamageDoneCommand = new MvxCommand(() => OpenEditMinDamageDone = true);
        ApplyMinDamageDoneCommand = new MvxCommand(ApplyMinDamageDone);
        OpenEditMinHealDoneCommand = new MvxCommand(() => OpenEditMinHealDone = true);
        ApplyMinHealDoneCommand = new MvxCommand(ApplyMinHealDone);
        OpenEditEnergyRecoveryCommand = new MvxCommand(() => OpenEditMinEnergyRecovery = true);
        ApplyMinEnergyRecoveryCommand = new MvxCommand(ApplyMinEnergyRecovery);

        OpenEditMinDPSCommand = new MvxCommand(() => OpenEditMinDPS = true);
        ApplyMinDPSCommand = new MvxCommand(ApplyMinDPS);
        OpenEditMinHPSCommand = new MvxCommand(() => OpenEditMinHPS = true);
        ApplyMinHPSCommand = new MvxCommand(ApplyMinHPS);
        OpenEditMinRPSCommand = new MvxCommand(() => OpenEditMinRPS = true);
        ApplyMinRPSCommand = new MvxCommand(ApplyMinRPS);

        UseFilterByMinDamageDoneCommand = new MvxCommand(() => UseFilterByMinDamageDone = !UseFilterByMinDamageDone);
        UseFilterByMinHealDoneCommand = new MvxCommand(() => UseFilterByMinHealDone = !UseFilterByMinHealDone);
        UseFilterByMinEnergyRecoveryCommand = new MvxCommand(() => UseFilterByMinEnergyRecovery = !UseFilterByMinEnergyRecovery);

        UseFilterByMinDPSCommand = new MvxCommand(() => UseFilterByMinDPS = !UseFilterByMinDPS);
        UseFilterByMinHPSCommand = new MvxCommand(() => UseFilterByMinHPS = !UseFilterByMinHPS);
        UseFilterByMinRPSCommand = new MvxCommand(() => UseFilterByMinRPS = !UseFilterByMinRPS);

        FilterClearCommand = new MvxCommand(ClearFilter);
    }

    #region Commands

    public IMvxCommand SwitchBetweenValuesCommand { get; set; }

    public IMvxCommand OpenEditMinDamageDoneCommand { get; set; }

    public IMvxCommand ApplyMinDamageDoneCommand { get; set; }

    public IMvxCommand OpenEditMinHealDoneCommand { get; set; }

    public IMvxCommand ApplyMinHealDoneCommand { get; set; }

    public IMvxCommand OpenEditEnergyRecoveryCommand { get; set; }

    public IMvxCommand ApplyMinEnergyRecoveryCommand { get; set; }

    public IMvxCommand UseFilterByMinDamageDoneCommand { get; set; }

    public IMvxCommand UseFilterByMinHealDoneCommand { get; set; }

    public IMvxCommand UseFilterByMinEnergyRecoveryCommand { get; set; }

    public IMvxCommand OpenEditMinDPSCommand { get; set; }

    public IMvxCommand ApplyMinDPSCommand { get; set; }

    public IMvxCommand OpenEditMinHPSCommand { get; set; }

    public IMvxCommand ApplyMinHPSCommand { get; set; }

    public IMvxCommand OpenEditMinRPSCommand { get; set; }

    public IMvxCommand ApplyMinRPSCommand { get; set; }

    public IMvxCommand UseFilterByMinDPSCommand { get; set; }

    public IMvxCommand UseFilterByMinHPSCommand { get; set; }

    public IMvxCommand UseFilterByMinRPSCommand { get; set; }

    public IMvxCommand FilterClearCommand { get; set; }

    #endregion

    #region Properties

    public bool ShowEffeciency { get; set; }

    public List<CombatPlayerModel> PlayersCombat
    {
        get { return _playersCombat; }
        set
        {
            SetProperty(ref _playersCombat, value);

            if (value.Count > 0)
            {
                SelectedPlayer = value[0];
            }
        }
    }

    public CombatPlayerModel SelectedPlayer
    {
        get { return _selectedPlayer; }
        set
        {
            SetProperty(ref _selectedPlayer, value);

            (BasicTemplate as BasicTemplateViewModel).Data = value;
            (BasicTemplate as BasicTemplateViewModel).PetsId = Combat != null ? Combat.PetsId : null;
        }
    }

    public CombatModel Combat
    {
        get { return _combat; }
        set
        {
            SetProperty(ref _combat, value);
        }
    }

    public int CombatInformationType
    {
        get { return _combatInformationType; }
        set
        {
            SetProperty(ref _combatInformationType, value);
        }
    }

    public List<string> FilterList
    {
        get { return _filterList; }
        set
        {
            SetProperty(ref _filterList, value);
        }
    }

    public int SeletedFilterIndex
    {
        get { return _selectedFilterIndex; }
        set
        {
            SetProperty(ref _selectedFilterIndex, value);
            UseFilter(value);
        }
    }

    public int MinDamageDone
    {
        get { return _minDamageDone; }
        set
        {
            SetProperty(ref _minDamageDone, value);
        }
    }

    public int MinHealDone
    {
        get { return _minHealDone; }
        set
        {
            SetProperty(ref _minHealDone, value);
        }
    }

    public int MinEnergyRecovery
    {
        get { return _minEnergyRecovery; }
        set
        {
            SetProperty(ref _minEnergyRecovery, value);
        }
    }

    public bool OpenEditMinDamageDone
    {
        get { return _openEditMinDamageDone; }
        set
        {
            SetProperty(ref _openEditMinDamageDone, value);
        }
    }

    public bool OpenEditMinHealDone
    {
        get { return _openEditMinHealDone; }
        set
        {
            SetProperty(ref _openEditMinHealDone, value);
        }
    }

    public bool OpenEditMinEnergyRecovery
    {
        get { return _openEditMinEnergyRecovery; }
        set
        {
            SetProperty(ref _openEditMinEnergyRecovery, value);
        }
    }

    public bool UseFilterByMinDamageDone
    {
        get { return _useFilterByMinDamageDone; }
        set
        {
            SetProperty(ref _useFilterByMinDamageDone, value);
            if (!value)
            {
                MinDamageDone = 0;
                SeletedFilterIndex = 0;
                ApplyMinDamageDone();
            }
        }
    }

    public bool UseFilterByMinHealDone
    {
        get { return _useFilterByMinHealDone; }
        set
        {
            SetProperty(ref _useFilterByMinHealDone, value);
            if (!value)
            {
                MinHealDone = 0;
                SeletedFilterIndex = 0;
                ApplyMinHealDone();
            }
        }
    }

    public bool UseFilterByMinEnergyRecovery
    {
        get { return _useFilterByMinEnergyRecovery; }
        set
        {
            SetProperty(ref _useFilterByMinEnergyRecovery, value);
            if (!value)
            {
                MinEnergyRecovery = 0;
                SeletedFilterIndex = 0;
                ApplyMinEnergyRecovery();
            }
        }
    }

    public int MinDPS
    {
        get { return _minDPS; }
        set
        {
            SetProperty(ref _minDPS, value);
        }
    }

    public int MinHPS
    {
        get { return _minHPS; }
        set
        {
            SetProperty(ref _minHPS, value);
        }
    }

    public int MinRPS
    {
        get { return _minRPS; }
        set
        {
            SetProperty(ref _minRPS, value);
        }
    }

    public bool OpenEditMinDPS
    {
        get { return _openEditMinDPS; }
        set
        {
            SetProperty(ref _openEditMinDPS, value);
        }
    }

    public bool OpenEditMinHPS
    {
        get { return _openEditMinHPS; }
        set
        {
            SetProperty(ref _openEditMinHPS, value);
        }
    }

    public bool OpenEditMinRPS
    {
        get { return _openEditMinRPS; }
        set
        {
            SetProperty(ref _openEditMinRPS, value);
        }
    }

    public bool UseFilterByMinDPS
    {
        get { return _useFilterByMinDPS; }
        set
        {
            SetProperty(ref _useFilterByMinDPS, value);
            if (!value)
            {
                MinDPS = 0;
                SeletedFilterIndex = 0;
                ApplyMinDPS();
            }
        }
    }

    public bool UseFilterByMinHPS
    {
        get { return _useFilterByMinHPS; }
        set
        {
            SetProperty(ref _useFilterByMinHPS, value);
            if (!value)
            {
                MinHPS = 0;
                SeletedFilterIndex = 0;
                ApplyMinHPS();
            }
        }
    }

    public bool UseFilterByMinRPS
    {
        get { return _useFilterByMinRPS; }
        set
        {
            SetProperty(ref _useFilterByMinRPS, value);
            if (!value)
            {
                MinRPS = 0;
                SeletedFilterIndex = 0;
                ApplyMinRPS();
            }
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

    public int TotalDamage
    {
        get { return _totalDamage; }
        set
        {
            SetProperty(ref _totalDamage, value);
        }
    }

    public int TotalHeal
    {
        get { return _totalHeal; }
        set
        {
            SetProperty(ref _totalHeal, value);
        }
    }

    public int TotalResoures
    {
        get { return _totalResources; }
        set
        {
            SetProperty(ref _totalResources, value);
        }
    }

    public double TotalDamagePerSecond
    {
        get { return _totalDamagePerSecond; }
        set
        {
            SetProperty(ref _totalDamagePerSecond, value);
        }
    }

    public double TotalHealPerSecond
    {
        get { return _totalHealPerSecond; }
        set
        {
            SetProperty(ref _totalHealPerSecond, value);
        }
    }

    public double TotalResourcesPerSecond
    {
        get { return _totalResourcesPerSecond; }
        set
        {
            SetProperty(ref _totalResourcesPerSecond, value);
        }
    }

    public bool ShowSummaryInformation
    {
        get { return _showSummaryInformation; }
        set
        {
            SetProperty(ref _showSummaryInformation, value);
        }
    }

    #endregion

    public void ApplyMinDamageDone()
    {
        if (MinDamageDone > 0)
        {
            FilterInformationByMinDamageDone(MinDamageDone);
            OpenEditMinDamageDone = false;

            return;
        }

        PlayersCombat = new List<CombatPlayerModel>(_mainPlayersCombat);
        if (MinHealDone > 0)
        {
            ApplyMinHealDone();
        }

        if (MinEnergyRecovery > 0)
        {
            ApplyMinEnergyRecovery();
        }

        OpenEditMinDamageDone = false;
    }

    public void ApplyMinHealDone()
    {
        if (MinHealDone > 0)
        {
            FilterInformationByMinHealDone(MinHealDone);
            OpenEditMinHealDone = false;

            return;
        }

        PlayersCombat = new List<CombatPlayerModel>(_mainPlayersCombat);
        if (MinDamageDone > 0)
        {
            ApplyMinDamageDone();
        }

        if (MinEnergyRecovery > 0)
        {
            ApplyMinEnergyRecovery();
        }

        OpenEditMinHealDone = false;
    }

    public void ApplyMinEnergyRecovery()
    {
        if (MinEnergyRecovery > 0)
        {
            FilterInformationByMinEnergyRecovery(MinEnergyRecovery);
            OpenEditMinEnergyRecovery = false;

            return;
        }

        PlayersCombat = new List<CombatPlayerModel>(_mainPlayersCombat);
        if (MinDamageDone > 0)
        {
            ApplyMinDamageDone();
        }

        if (MinHealDone > 0)
        {
            ApplyMinHealDone();
        }

        OpenEditMinEnergyRecovery = false;
    }

    public void ApplyMinDPS()
    {
        if (MinDPS > 0)
        {
            FilterInformationByMinDPS(MinDPS);
            OpenEditMinDPS = false;

            return;
        }

        PlayersCombat = new List<CombatPlayerModel>(_mainPlayersCombat);
        if (MinHPS > 0)
        {
            ApplyMinHPS();
        }

        if (MinRPS > 0)
        {
            ApplyMinRPS();
        }

        OpenEditMinDPS = false;
    }

    public void ApplyMinHPS()
    {
        if (MinHPS > 0)
        {
            FilterInformationByMinHPS(MinHPS);
            OpenEditMinHPS = false;

            return;
        }

        PlayersCombat = new List<CombatPlayerModel>(_mainPlayersCombat);
        if (MinDPS > 0)
        {
            ApplyMinDPS();
        }

        if (MinRPS > 0)
        {
            ApplyMinRPS();
        }

        OpenEditMinHPS = false;
    }

    public void ApplyMinRPS()
    {
        if (MinRPS > 0)
        {
            FilterInformationByMinRPS(MinRPS);
            OpenEditMinRPS = false;

            return;
        }

        PlayersCombat = new List<CombatPlayerModel>(_mainPlayersCombat);
        if (MinDPS > 0)
        {
            ApplyMinDPS();
        }

        if (MinHPS > 0)
        {
            ApplyMinHPS();
        }

        OpenEditMinRPS = false;
    }

    public void SwitchBetweenValues(int type)
    {
        switch (type)
        {
            case 0:
                GetTotalValueFiltersName();
                break;
            case 1:
                GetValuePerSecondFiltersName();
                break;
            default:
                break;
        }

        CombatInformationType = type;
        ClearFilter();
    }

    public override void ViewAppeared()
    {
        GetTotalValueFiltersName();

        base.ViewAppeared();
    }

    protected override void ChildPrepare(CombatModel parameter)
    {
        PlayersCombat = parameter.Players;
        Combat = parameter;
        _mainPlayersCombat = parameter.Players;

        ShowEffeciency = PlayersCombat.Any(x => x.PlayerParseInfo != null);

        SelectedPlayer = PlayersCombat[0];

        var damageDone = PlayersCombat.Average(x => x.DamageDone);
        var healDone = PlayersCombat.Average(x => x.HealDone);
        var energyRecovery = PlayersCombat.Average(x => x.ResourcesRecovery);

        AverageDamage = double.Round(damageDone, 2);
        AverageHeal = double.Round(healDone, 2);
        AverageResources = double.Round(energyRecovery, 2);

        AverageDamagePerSecond = PlayersCombat.Average(x => x.DamageDonePerSecond);
        AverageHealPerSecond = PlayersCombat.Average(x => x.HealDonePerSecond);
        AverageResourcesPerSecond = PlayersCombat.Average(x => x.EnergyRecoveryPerSecond);

        TotalDamage = PlayersCombat.Sum(x => x.DamageDone);
        TotalHeal = PlayersCombat.Sum(x => x.HealDone);
        TotalResoures = PlayersCombat.Sum(x => x.ResourcesRecovery);

        TotalDamagePerSecond = PlayersCombat.Sum(x => x.DamageDonePerSecond);
        TotalHealPerSecond = PlayersCombat.Sum(x => x.HealDonePerSecond);
        TotalResourcesPerSecond = PlayersCombat.Sum(x => x.EnergyRecoveryPerSecond);
    }

    private void GetTotalValueFiltersName()
    {
        var minDamage = TranslationSource.Instance["CombatAnalysis.App.Localizations.Resources.DetailsSpecificalCombat.Resource.MinDamage"];
        var minHeal = TranslationSource.Instance["CombatAnalysis.App.Localizations.Resources.DetailsSpecificalCombat.Resource.MinHeal"];
        var minResurces = TranslationSource.Instance["CombatAnalysis.App.Localizations.Resources.DetailsSpecificalCombat.Resource.MinResources"];

        FilterList = new List<string> { "", minDamage, minHeal, minResurces };
    }

    private void GetValuePerSecondFiltersName()
    {
        var minDPS = TranslationSource.Instance["CombatAnalysis.App.Localizations.Resources.DetailsSpecificalCombat.Resource.MinDPS"];
        var minHPS = TranslationSource.Instance["CombatAnalysis.App.Localizations.Resources.DetailsSpecificalCombat.Resource.MinHPS"];
        var minRPS = TranslationSource.Instance["CombatAnalysis.App.Localizations.Resources.DetailsSpecificalCombat.Resource.MinRPS"];

        FilterList = new List<string> { "", minDPS, minHPS, minRPS };
    }

    private void UseFilter(int index)
    {
        switch (index)
        {
            case 1:
                if (CombatInformationType == 0)
                {
                    UseFilterByMinDamageDone = true;
                }
                else
                {
                    UseFilterByMinDPS = true;
                }
                break;
            case 2:
                if (CombatInformationType == 0)
                {
                    UseFilterByMinHealDone = true;
                }
                else
                {
                    UseFilterByMinHPS = true;
                }
                break;
            case 3:
                if (CombatInformationType == 0)
                {
                    UseFilterByMinEnergyRecovery = true;
                }
                else
                {
                    UseFilterByMinRPS = true;
                }
                break;
            default:
                break;
        }
    }

    private void ClearFilter()
    {
        UseFilterByMinDamageDone = false;
        UseFilterByMinHealDone = false;
        UseFilterByMinEnergyRecovery = false;

        MinDamageDone = 0;
        MinHealDone = 0;
        MinEnergyRecovery = 0;

        PlayersCombat = new List<CombatPlayerModel>(_mainPlayersCombat);
    }

    private void FilterInformationByMinDamageDone(int minDamageDone)
    {
        var temporaryPlayersCombat = new List<CombatPlayerModel>();
        foreach (var player in PlayersCombat)
        {
            if (player.DamageDone >= minDamageDone)
            {
                temporaryPlayersCombat.Add(player);
            }
        }

        PlayersCombat = new List<CombatPlayerModel>(temporaryPlayersCombat);
    }

    private void FilterInformationByMinHealDone(int minHealDone)
    {
        var temporaryPlayersCombat = new List<CombatPlayerModel>();
        foreach (var player in PlayersCombat)
        {
            if (player.HealDone >= minHealDone)
            {
                temporaryPlayersCombat.Add(player);
            }
        }

        PlayersCombat = new List<CombatPlayerModel>(temporaryPlayersCombat);
    }

    private void FilterInformationByMinEnergyRecovery(int minEnergyRecovery)
    {
        var temporaryPlayersCombat = new List<CombatPlayerModel>();
        foreach (var player in PlayersCombat)
        {
            if (player.ResourcesRecovery >= minEnergyRecovery)
            {
                temporaryPlayersCombat.Add(player);
            }
        }

        PlayersCombat = new List<CombatPlayerModel>(temporaryPlayersCombat);
    }

    private void FilterInformationByMinDPS(int minDPS)
    {
        var temporaryPlayersCombat = new List<CombatPlayerModel>();
        foreach (var player in PlayersCombat)
        {
            if (player.DamageDonePerSecond >= minDPS)
            {
                temporaryPlayersCombat.Add(player);
            }
        }

        PlayersCombat = new List<CombatPlayerModel>(temporaryPlayersCombat);
    }

    private void FilterInformationByMinHPS(int minHPS)
    {
        var temporaryPlayersCombat = new List<CombatPlayerModel>();
        foreach (var player in PlayersCombat)
        {
            if (player.HealDonePerSecond >= minHPS)
            {
                temporaryPlayersCombat.Add(player);
            }
        }

        PlayersCombat = new List<CombatPlayerModel>(temporaryPlayersCombat);
    }

    private void FilterInformationByMinRPS(int minRPS)
    {
        var temporaryPlayersCombat = new List<CombatPlayerModel>();
        foreach (var player in PlayersCombat)
        {
            if (player.EnergyRecoveryPerSecond >= minRPS)
            {
                temporaryPlayersCombat.Add(player);
            }
        }

        PlayersCombat = new List<CombatPlayerModel>(temporaryPlayersCombat);
    }
}
