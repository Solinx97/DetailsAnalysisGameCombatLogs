using CombatAnalysis.Core.Models;
using CombatAnalysis.Core.ViewModels.Base;
using MvvmCross.Commands;

namespace CombatAnalysis.Core.ViewModels;

public class DetailsSpecificalCombatViewModel : ParentTemplate<CombatModel>
{
    private CombatModel _combat;
    private List<CombatPlayerModel> _playersCombat;
    private CombatPlayerModel _selectedPlayer;
    private int _combatInformationType;
    private TimeSpan _duration;

    public DetailsSpecificalCombatViewModel()
    {
        BasicTemplate.Parent = this;
        BasicTemplate.Handler.PropertyUpdate<BasicTemplateViewModel>(BasicTemplate, nameof(BasicTemplateViewModel.Step), 2);

        SwitchBetweenValuesCommand = new MvxCommand<int>((type) => CombatInformationType = type);
    }

    #region Commands

    public IMvxCommand SwitchBetweenValuesCommand { get; set; }

    #endregion

    #region Properties

    public List<CombatPlayerModel> PlayersCombat
    {
        get { return _playersCombat; }
        set
        {
            SetProperty(ref _playersCombat, value);

            SelectedPlayer = value[0];
        }
    }

    public CombatPlayerModel SelectedPlayer
    {
        get { return _selectedPlayer; }
        set
        {
            SetProperty(ref _selectedPlayer, value);

            (BasicTemplate as BasicTemplateViewModel).Data = value;
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

    #endregion

    protected override void ChildPrepare(CombatModel parameter)
    {
        PlayersCombat = parameter.Players;
        Combat = parameter;

        if (!TimeSpan.TryParse(parameter.Duration, out _duration))
        {
            _duration = TimeSpan.Zero;
            return;
        }

        foreach (var item in PlayersCombat)
        {
            item.DamageDonePerSecond = item.DamageDone / _duration.TotalSeconds;
            item.HealDonePerSecond = item.HealDone / _duration.TotalSeconds;
            item.EnergyRecoveryPerSecond = item.EnergyRecovery / _duration.TotalSeconds;
        }
    }
}
