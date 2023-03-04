using CombatAnalysis.Core.Models;
using CombatAnalysis.Core.ViewModels.Base;

namespace CombatAnalysis.Core.ViewModels;

public class DetailsSpecificalCombatViewModel : ParentTemplate<CombatModel>
{
    private CombatModel _combat;
    private List<CombatPlayerModel> _playersCombat;
    private CombatPlayerModel _selectedPlayer;

    public DetailsSpecificalCombatViewModel()
    {
        BasicTemplate.Parent = this;
        BasicTemplate.Handler.PropertyUpdate<BasicTemplateViewModel>(BasicTemplate, nameof(BasicTemplateViewModel.Step), 2);
    }

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

    #endregion

    protected override void ChildPrepare(CombatModel parameter)
    {
        PlayersCombat = parameter.Players;
        Combat = parameter;
    }
}
