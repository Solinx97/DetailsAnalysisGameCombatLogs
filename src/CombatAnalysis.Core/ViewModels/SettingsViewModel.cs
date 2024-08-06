using CombatAnalysis.Core.Localizations;
using CombatAnalysis.Core.ViewModels.Base;
using CombatAnalysis.Core.ViewModels.Settings;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Text.Json;

namespace CombatAnalysis.Core.ViewModels;

public class SettingsViewModel : ParentTemplate
{
    private ObservableCollection<Language> _languages;
    private Language _selectedLanguage;
    private string _logsLocation;

    public SettingsViewModel()
    {
        BasicTemplate.Parent = this;

        var userSettings = ReadUserSettings("user.json");
        _logsLocation = userSettings.Location;

        LanguageInit();
    }

    #region Properties

    public ObservableCollection<Language> Languages
    {
        get { return _languages; }
        set
        {
            SetProperty(ref _languages, value);
        }
    }

    public Language SelectedLanguage
    {
        get { return _selectedLanguage; }
        set
        {
            TranslationSource.Instance.Language = value.ToString().ToLower(CultureInfo.CurrentCulture);
            SetProperty(ref _selectedLanguage, value);
        }
    }

    public string LogsLocation
    {
        get { return _logsLocation; }
        set
        {
            SetProperty(ref _logsLocation, value);
        }
    }

    #endregion

    private void LanguageInit()
    {
        var currentCultureTag = CultureInfo.CurrentCulture.IetfLanguageTag;

        SelectedLanguage = currentCultureTag switch
        {
            "ru" => Language.RU,
            _ => Language.EN,
        };

        Languages = new ObservableCollection<Language> { Language.EN, Language.RU, };
    }

    private static UserSettings ReadUserSettings(string settingsName)
    {
        var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        using var fs = new FileStream($"{baseDirectory}{settingsName}", FileMode.OpenOrCreate);
        var userSettings = JsonSerializer.Deserialize<UserSettings>(fs);

        return userSettings;
    }
}
