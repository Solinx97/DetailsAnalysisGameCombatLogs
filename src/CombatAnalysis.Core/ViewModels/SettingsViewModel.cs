using CombatAnalysis.Core.Localizations;
using CombatAnalysis.Core.ViewModels.Base;
using System.Collections.ObjectModel;
using System.Globalization;

namespace CombatAnalysis.Core.ViewModels;

public class SettingsViewModel : ParentTemplate
{
    private ObservableCollection<Language> _languages;
    private Language _selectedLanguage;

    public SettingsViewModel()
	{
        BasicTemplate.Parent = this;

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
}
