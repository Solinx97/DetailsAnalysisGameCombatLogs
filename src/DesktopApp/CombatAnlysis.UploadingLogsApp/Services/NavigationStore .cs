using CombatAnalysis.UploadingLogsApp.ViewModels;
using CommunityToolkit.Mvvm.ComponentModel;

namespace CombatAnalysis.UploadingLogsApp.Services;

public partial class NavigationStore : ObservableObject
{
    [ObservableProperty]
    private ViewModelBase? currentViewModel;
}
