using CombatAnalysis.UploadingLogsApp.Localizations.Indexes;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace CombatAnalysis.UploadingLogsApp.Localizations;

public class LocalizationService : INotifyPropertyChanged
{
    private CultureInfo _currentCulture = CultureInfo.CurrentUICulture;

    public event PropertyChangedEventHandler? PropertyChanged;

    public static LocalizationService Instance { get; } = new();

    public CultureInfo CurrentCulture => _currentCulture;

    #region Localization resources

    public MainLocalization Main { get; } = new();

    public ParsingCombatLogsLocalization ParsingCombatLogs { get; } = new();

    #endregion

    public void SetLanguage(string language)
    {
        _currentCulture = new CultureInfo(language);

        CultureInfo.CurrentCulture = _currentCulture;
        CultureInfo.CurrentUICulture = _currentCulture;

        OnPropertyChanged(null);
    }

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(
            this,
            new PropertyChangedEventArgs(propertyName)
        );

        Main.NotifyLanguageChanged();
        ParsingCombatLogs.NotifyLanguageChanged();
    }
}
