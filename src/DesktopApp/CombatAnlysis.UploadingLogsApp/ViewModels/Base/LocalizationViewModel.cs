using CombatAnalysis.UploadingLogsApp.Enums;
using CombatAnalysis.UploadingLogsApp.Localizations;
using CommunityToolkit.Mvvm.ComponentModel;

namespace CombatAnalysis.UploadingLogsApp.ViewModels.Base;

public partial class LocalizationViewModel : ViewModelBase
{
    [ObservableProperty]
    public partial LocalizationService Localization { get; set; } = LocalizationService.Instance;
}
