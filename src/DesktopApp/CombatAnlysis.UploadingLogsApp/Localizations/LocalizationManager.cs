using System;
using System.Globalization;

namespace CombatAnalysis.UploadingLogsApp.Localizations;

public static class LocalizationManager
{
    public static event Action? LanguageChanged;

    public static CultureInfo CurrentCulture
    {
        get => CultureInfo.CurrentUICulture;
        set
        {
            CultureInfo.CurrentCulture = value;
            CultureInfo.CurrentUICulture = value;

            LanguageChanged?.Invoke();
        }
    }

    public static void SetLanguage(string culture)
    {
        CurrentCulture = new CultureInfo(culture);
    }
}
