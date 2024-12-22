using System.ComponentModel;
using System.Globalization;
using System.Resources;

namespace CombatAnalysis.Core.Localizations;

public class TranslationSource : INotifyPropertyChanged
{
    private readonly Dictionary<string, ResourceManager> _resourceManagerDictionary = new();
    private string _language = Thread.CurrentThread.CurrentUICulture.Name;

    public event PropertyChangedEventHandler? PropertyChanged;

    public static TranslationSource Instance { get; } = new TranslationSource();

    public string Language
    {
        get => _language;

        set
        {
            if (_language != null)
            {
                _language = value;

                var cultureInfo = new CultureInfo(value);
                Thread.CurrentThread.CurrentUICulture = cultureInfo;
                Thread.CurrentThread.CurrentCulture = cultureInfo;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(string.Empty));
            }
        }
    }

    public string this[string key]
    {
        get
        {
            var dataOfLocalization = GetDataOfLocalization(key);
            var translation = string.Empty;
            if (_resourceManagerDictionary.TryGetValue(dataOfLocalization.Item1, out var value))
            {
                translation = value.GetString(dataOfLocalization.Item2, Thread.CurrentThread.CurrentUICulture);
            }

            return translation ?? key;
        }
    }

    public void AddResourceManager(ResourceManager resourceManager)
    {
        if (resourceManager != null && !_resourceManagerDictionary.ContainsKey(resourceManager.BaseName))
        {
            _resourceManagerDictionary.Add(resourceManager.BaseName, resourceManager);
        }
    }

    private static Tuple<string, string> GetDataOfLocalization(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
        {
            throw new ArgumentNullException(nameof(path));
        }

        var indexPath = path.ToString(CultureInfo.CurrentCulture).LastIndexOf(".", StringComparison.CurrentCulture);
        var dataOfLocalization = new Tuple<string, string>(path[..indexPath], path[(indexPath + 1)..]);

        return dataOfLocalization;
    }
}
