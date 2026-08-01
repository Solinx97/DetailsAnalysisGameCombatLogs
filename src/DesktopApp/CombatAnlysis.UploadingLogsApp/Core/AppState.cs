using CommunityToolkit.Mvvm.ComponentModel;

namespace CombatAnalysis.UploadingLogsApp.Core;

public partial class AppState : ObservableObject
{
    [ObservableProperty]
    private bool isAuth;
}
