using CombatAnalysis.Core.ViewModels;
using MvvmCross.Platforms.Wpf.Views;
using System.Windows.Controls;

namespace CombatAnalysis.App.Views;

public partial class CombatPlayersView : MvxWpfView
{
    public CombatPlayersView()
    {
        InitializeComponent();

        DataContextChanged += DetailsSpecificalCombatView_DataContextChanged;
    }

    private void DetailsSpecificalCombatView_DataContextChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
    {
        var view = (CombatPlayersViewModel)ViewModel;

        UpdateColumnVisibility(view.ShowEffeciency);
    }

    private void UpdateColumnVisibility(bool showSpecialColumn)
    {
        var grid = (DataGrid)dataGrid1;
        var damageEfficiencyCollumn = (DataGridColumn)damageEfficiency;
        var healEfficiencyCollumn = (DataGridColumn)healEfficiency;

        if (showSpecialColumn)
        {
            if (!grid.Columns.Contains(damageEfficiencyCollumn))
            {
                grid.Columns.Add(damageEfficiencyCollumn);
            }

            if (!grid.Columns.Contains(healEfficiencyCollumn))
            {
                grid.Columns.Add(healEfficiencyCollumn);
            }
        }
        else
        {
            if (grid.Columns.Contains(damageEfficiencyCollumn))
            {
                grid.Columns.Remove(damageEfficiencyCollumn);
            }

            if (grid.Columns.Contains(healEfficiencyCollumn))
            {
                grid.Columns.Remove(healEfficiencyCollumn);
            }
        }
    }
}
