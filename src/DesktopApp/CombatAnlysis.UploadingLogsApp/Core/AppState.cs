using CombatAnalysis.UploadingLogsApp.Models.User;
using CommunityToolkit.Mvvm.ComponentModel;

namespace CombatAnalysis.UploadingLogsApp.Core;

public partial class AppState : ObservableObject
{
    [ObservableProperty]
    private string appName;

    [ObservableProperty]
    private string appVersion;

    [ObservableProperty]
    private bool isAuth;

    [ObservableProperty]
    private AppUserModel? user;

    [ObservableProperty]
    private bool allowLogout = true;
}
