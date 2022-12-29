using CombatAnalysis.Core.Consts;
using CombatAnalysis.Core.Interfaces;
using CombatAnalysis.Core.Models;
using MvvmCross.ViewModels;

namespace CombatAnalysis.Core.ViewModels;

public class DetailsSpecificalCombatViewModel : MvxViewModel<CombatModel>
{
    private CombatModel _combat;
    private IImprovedMvxViewModel _basicTemplate;
    private List<CombatPlayerModel> _playersCombatData;
    private CombatPlayerModel _selectedPlayer;

    public DetailsSpecificalCombatViewModel()
    {
        BasicTemplate = Templates.Basic;
        BasicTemplate.Parent = this;
        BasicTemplate.Handler.PropertyUpdate<BasicTemplateViewModel>(BasicTemplate, "Step", 2);
    }

    #region Properties

    public IImprovedMvxViewModel BasicTemplate
    {
        get { return _basicTemplate; }
        set
        {
            SetProperty(ref _basicTemplate, value);
        }
    }

    public List<CombatPlayerModel> PlayersCombatData
    {
        get { return _playersCombatData; }
        set
        {
            SetProperty(ref _playersCombatData, value);

            SelectedPlayer = value[0];
        }
    }

    public CombatPlayerModel SelectedPlayer
    {
        get { return _selectedPlayer; }
        set
        {
            SetProperty(ref _selectedPlayer, value);

            BasicTemplate.Handler.Data = value;
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

    public override void Prepare(CombatModel parameter)
    {
        Combat = parameter;
    }
}
