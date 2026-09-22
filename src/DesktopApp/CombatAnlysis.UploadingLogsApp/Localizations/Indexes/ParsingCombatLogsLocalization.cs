using System.ComponentModel;

namespace CombatAnalysis.UploadingLogsApp.Localizations.Indexes;

public class ParsingCombatLogsLocalization : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    public string this[string key]
    {
        get
        {
            return Resources.ParsingCombatLogs.Resource.ResourceManager.GetString(
                       key,
                       LocalizationService.Instance.CurrentCulture)
                   ?? key;
        }
    }

    internal void NotifyLanguageChanged()
    {
        PropertyChanged?.Invoke(
            this,
            new PropertyChangedEventArgs(null));
    }
}
