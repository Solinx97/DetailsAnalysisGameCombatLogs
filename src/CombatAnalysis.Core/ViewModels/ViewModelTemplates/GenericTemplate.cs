using CombatAnalysis.Core.Interfaces;
using MvvmCross.ViewModels;

namespace CombatAnalysis.Core.ViewModels.ViewModelTemplates;

abstract public class GenericTemplate<TParameter> : MvxViewModel<TParameter>
    where TParameter : notnull
{
    private IImprovedMvxViewModel _basicTemplate;
    private bool _isShowFilters;
    private string _selectedDamageDoneSource;
    private string _selectedPlayer;
    private long _totalValue;

    #region Properties

    public IImprovedMvxViewModel BasicTemplate
    {
        get { return _basicTemplate; }
        set
        {
            SetProperty(ref _basicTemplate, value);
        }
    }

    public string SelectedSource
    {
        get { return _selectedDamageDoneSource; }
        set
        {
            SetProperty(ref _selectedDamageDoneSource, value);

            Filter();
        }
    }

    public string SelectedPlayer
    {
        get { return _selectedPlayer; }
        set
        {
            SetProperty(ref _selectedPlayer, value);
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

    public bool IsShowFilters
    {
        get { return _isShowFilters; }
        set
        {
            SetProperty(ref _isShowFilters, value);

            if (!value)
            {
                TurnOnAllFilters();
            }
        }
    }

    #endregion

    public override void Prepare(TParameter parameter)
    {
        Task.Run(() => ChildPrepareAsync(parameter));
    }

    protected abstract Task ChildPrepareAsync(TParameter parameter);

    protected abstract void Filter();

    protected abstract Task LoadDetailsAsync(int combatPlayerId);

    protected abstract Task LoadGenericDetailsAsync(int combatPlayerId);

    protected abstract void GetDetails();

    protected abstract void TurnOnAllFilters();
}