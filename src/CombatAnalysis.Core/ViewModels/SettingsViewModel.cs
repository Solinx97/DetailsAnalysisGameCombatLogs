﻿using CombatAnalysis.Core.Localizations;
using CombatAnalysis.Core.Settings;
using CombatAnalysis.Core.ViewModels.Base;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Text.Json;

namespace CombatAnalysis.Core.ViewModels;

public class SettingsViewModel : ParentTemplate
{
    private readonly IMvxNavigationService _mvvmNavigation;

    private ObservableCollection<Language>? _languages;
    private Language _selectedLanguage;
    private string _logsLocation;

    public SettingsViewModel(IMvxNavigationService mvvmNavigation)
    {
        Basic.Parent = this;

        _mvvmNavigation = mvvmNavigation;

        var userSettings = ReadUserSettings("User.json");
        _logsLocation = userSettings?.Location ?? string.Empty;

        CloseCommand = new MvxAsyncCommand(Close);

        LanguageInit();
    }

    #region Command

    public IMvxAsyncCommand CloseCommand { get; set; }

    #endregion

    #region View model properties

    public ObservableCollection<Language>? Languages
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

    private async Task Close()
    {
        await _mvvmNavigation.Close(this);
    }

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

    private static UserSettings? ReadUserSettings(string settingsName)
    {
        var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        using var fs = new FileStream($"{baseDirectory}{settingsName}", FileMode.OpenOrCreate);
        var userSettings = JsonSerializer.Deserialize<UserSettings>(fs);

        return userSettings;
    }
}
