using System.ComponentModel;

namespace CombatAnalysis.UploadingLogsApp.Localizations.Indexes;

public class MainLocalization : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    public string this[string key]
    {
        get
        {
            return Resources.Main.Resource.ResourceManager.GetString(
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
