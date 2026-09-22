using CombatAnalysis.Core.Models.GameLogs;
using CombatAnalysis.Core.ViewModels.Base;
using CombatAnalysis.Core.ViewModels.ViewModelTemplates;
using MvvmCross.Commands;

namespace CombatAnalysis.Core.ViewModels.CombatPlayers;

public abstract class BasicCombatPlayerViewModel : ParentTemplate<List<CombatPlayerModel>>
{
    protected List<CombatPlayerModel>? _defaultPlayers;

    private long _bestValue;
    private CombatModel? _combat;
    private List<CombatPlayerModel>? _players;
    private CombatPlayerModel? _selectedPlayer;
    private int _minValue;
    private bool _openEditMinValue;
    private int _minVPS;
    private bool _openEditMinVPS;
    private double _averageValue;
    private double _averageVPS;
    private long _totalValue;
    private double _totalVPS;

    public BasicCombatPlayerViewModel()
    {
        OpenEditMinValueCommand = new MvxCommand(() => OpenEditMinValue = !OpenEditMinValue);
        ApplyMinValueCommand = new MvxCommand(CloseEditMinValue);

        OpenEditMinVPSCommand = new MvxCommand(() => OpenEditMinVPS = !OpenEditMinVPS);
        ApplyMinVPSCommand = new MvxCommand(CloseEditMinVPS);

        ClearMinValueCommand = new MvxCommand(ClearValueFilter);
        ClearMinVPSCommand = new MvxCommand(ClearVPSFilter);
    }

    #region Commands

    public IMvxCommand OpenEditMinValueCommand { get; }

    public IMvxCommand ApplyMinValueCommand { get; }

    public IMvxCommand ClearMinValueCommand { get; }

    public IMvxCommand OpenEditMinVPSCommand { get; }

    public IMvxCommand ApplyMinVPSCommand { get; }

    public IMvxCommand ClearMinVPSCommand { get; }

    #endregion

    #region View model properties

    public long BestValue
    {
        get { return _bestValue; }
        set
        {
            SetProperty(ref _bestValue, value);
        }
    }

    public List<CombatPlayerModel>? Players
    {
        get => _players;
        set
        {
            SetProperty(ref _players, value);

            if (value != null && value.Count > 0)
            {
                SelectedPlayer = value[0];
            }
        }
    }

    public CombatPlayerModel? SelectedPlayer
    {
        get => _selectedPlayer;
        set
        {
            SetProperty(ref _selectedPlayer, value);

            if (value != null)
            {
                if (Basic is BasicTemplateViewModel basicTemplateViewModel)
                {
                    basicTemplateViewModel.Data = value;
                    basicTemplateViewModel.PetsId = (Combat?.PetsId) ?? [];
                }
            }
        }
    }

    public CombatModel? Combat
    {
        get => _combat;
        set
        {
            SetProperty(ref _combat, value);
        }
    }

    public int MinValue
    {
        get { return _minValue; }
        set
        {
            SetProperty(ref _minValue, value);
        }
    }

    public bool OpenEditMinValue
    {
        get { return _openEditMinValue; }
        set
        {
            SetProperty(ref _openEditMinValue, value);
        }
    }

    public int MinVPS
    {
        get { return _minVPS; }
        set
        {
            SetProperty(ref _minVPS, value);
        }
    }

    public bool OpenEditMinVPS
    {
        get { return _openEditMinVPS; }
        set
        {
            SetProperty(ref _openEditMinVPS, value);
        }
    }

    public double AverageValue
    {
        get { return _averageValue; }
        set
        {
            SetProperty(ref _averageValue, value);
        }
    }

    public double AverageVPS
    {
        get { return _averageVPS; }
        set
        {
            SetProperty(ref _averageVPS, value);
        }
    }

    public long TotalValue
    {
        get { return _totalValue; }
        set
        {
            SetProperty(ref _totalValue, value);
        }
    }

    public double TotalVPS
    {
        get { return _totalVPS; }
        set
        {
            SetProperty(ref _totalVPS, value);
        }
    }

    #endregion

    public int ValueType { get; protected set; }

    public override void Prepare(List<CombatPlayerModel> parameter)
    {
        base.Prepare();
    }

    public abstract void OrderBy(int tabIndex);

    private void CloseEditMinValue()
    {
        OpenEditMinValue = !OpenEditMinValue;
        ApplyMinValue();
    }

    public void ApplyMinValue()
    {
        if (MinValue > 0)
        {
            FilterByMinValue(MinValue);

            return;
        }

        Players = _defaultPlayers != null ? [.. _defaultPlayers] : [];
    }

    private void FilterByMinValue(long minValue)
    {
        if (_defaultPlayers == null || Players == null)
        {
            return;
        }

        var filteredPlayers = new List<CombatPlayerModel>();
        var defaultCollection = MinVPS > 0 ? Players : _defaultPlayers;
        foreach (var player in defaultCollection)
        {
            long value = 0;
            switch (ValueType)
            {
                case 0:
                    value = player.Unit.UnitInfo.DamageDone;
                    break;
                case 1:
                    value = player.Unit.UnitInfo.HealDone;
                    break;
                case 2:
                    value = player.Unit.UnitInfo.DamageTaken;
                    break;
                case 3:
                    value = player.Unit.UnitInfo.ResourcesRecovery;
                    break;
                default:
                    break;
            }

            if (value >= minValue)
            {
                filteredPlayers.Add(player);
            }
        }

        Players = [.. filteredPlayers];
    }

    private void ClearValueFilter()
    {
        MinValue = 0;

        if (MinVPS > 0)
        {
            ApplyMinVPS();
        }
        else
        {
            Players = _defaultPlayers != null ? [.. _defaultPlayers] : [];
        }
    }

    private void CloseEditMinVPS()
    {
        OpenEditMinVPS = !OpenEditMinVPS;
        ApplyMinVPS();
    }

    public void ApplyMinVPS()
    {
        if (MinVPS > 0)
        {
            FilterByMinVPS(MinVPS);

            return;
        }

        Players = _defaultPlayers != null ? [.. _defaultPlayers] : [];
    }

    private void FilterByMinVPS(int minVPS)
    {
        if (_defaultPlayers == null || Players == null)
        {
            return;
        }

        var filteredPlayers = new List<CombatPlayerModel>();
        var defaultCollection = MinValue > 0 ? Players : _defaultPlayers;
        foreach (var player in defaultCollection)
        {
            var value = 0.0;
            switch (ValueType)
            {
                case 0:
                    value = player.DamageDonePerSecond;
                    break;
                case 1:
                    value = player.HealDonePerSecond;
                    break;
                case 2:
                    value = player.DamageTakenPerSecond;
                    break;
                case 3:
                    value = player.ResourcesRecoveryPerSecond;
                    break;
                default:
                    break;
            }

            if (value >= minVPS)
            {
                filteredPlayers.Add(player);
            }
        }

        Players = [.. filteredPlayers];
    }

    private void ClearVPSFilter()
    {
        MinVPS = 0;

        if (MinValue > 0)
        {
            ApplyMinValue();
        }
        else
        {
            Players = _defaultPlayers != null ? [.. _defaultPlayers] : [];
        }
    }
}
