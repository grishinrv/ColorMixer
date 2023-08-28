using ColorMixer.Contracts.Helpers;
using ColorMixer.Contracts.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using ColorMixer.Contracts.Localization;
using ColorMixer.Application.Services;
using System.Linq;
using System;

namespace ColorMixer.Application.ViewModels
{
    public sealed partial class SettingsViewModel : ObservableObject, IViewModelInitializable
    {
        private readonly ISettingsRepository _settingsRepository;
        private readonly ThemeSettingsManager _themeSettingsManager;

#pragma warning disable CS8618
        public SettingsViewModel(ISettingsRepository settingsRepository, ThemeSettingsManager themeSettingsManager)
#pragma warning restore CS8618
        {
            _settingsRepository = settingsRepository;
            _themeSettingsManager = themeSettingsManager;
        }

        public async Task OnFirstOpen()
        {
            SelectedTheme = await _settingsRepository.GetSetting<string>(SettingsHelper.SELECTED_THEME);
            DarkMode = await _settingsRepository.GetSetting<bool>(SettingsHelper.DARK_MODE);
            HighContrast = await _settingsRepository.GetSetting<bool>(SettingsHelper.HIGHT_CONTRAST);
            UseOsTheme = await _settingsRepository.GetSetting<bool>(SettingsHelper.USE_OS_THEME);
            string currentUiCulture = await _settingsRepository.GetSetting<string>(SettingsHelper.SELECTED_UI_CULTURE);
            SelectedCultureIndex = Array.IndexOf(Cultures, Cultures.First(x => x.Name == currentUiCulture));
        }

        [ObservableProperty]
        private string _selectedTheme;

        [ObservableProperty]
        private bool _darkMode;

        [ObservableProperty]
        private bool _highContrast;

        [ObservableProperty]
        private bool _useOsTheme;

        [ObservableProperty]
        private CultureInfo _selectedUiCulture;

        /// <summary>
        /// MahApps Metro themes list: 
        /// https://mahapps.com/docs/themes/usage
        /// </summary>
        public IReadOnlyList<string> Themes { get; } = new List<string>()
        {
            "Red", "Green", "Blue", "Purple", "Orange", "Lime", "Emerald", "Teal", "Cyan", "Cobalt", "Indigo",
            "Violet", "Pink", "Magenta", "Crimson", "Amber", "Yellow", "Brown", "Olive", "Steel", "Mauve", "Taupe", "Sienna"
        };


        public IReadOnlyList<string> Languages { get; } = new List<string>()
        {
            "Русский язык", "Deutch", "English"
        };

        /// <summary>
        /// Supported cultures: 
        /// <see cref="Translation.Designer"/>
        /// </summary>
        private CultureInfo[] Cultures = new CultureInfo[]
        { 
            new CultureInfo("ru-Ru"),
            new CultureInfo("de-De"),
            new CultureInfo("en-Us"),
        }; 

        private int _selectedCultureIndex;

        /// <summary>
        /// Culture should be applied at once, othervise user will not be able to find proper button
        /// </summary>
        public int SelectedCultureIndex
        {
            get => _selectedCultureIndex;
            set
            {
                SetProperty(ref _selectedCultureIndex, value);
                SelectedUiCulture = Cultures[value];
                LocalizationService.Instance.SetCulture(Cultures[value]);
            }
        }

        [RelayCommand]
        private async Task ApplyChanges()
        {
            await ApplyThemeSettingsIfNeeded();
            await _settingsRepository.SaveSettingIfChanged<string>(SettingsHelper.SELECTED_UI_CULTURE, SelectedUiCulture.Name);
        }


        private async Task ApplyThemeSettingsIfNeeded()
        {
            if (!await _settingsRepository.SaveSettingIfChanged<string>(SettingsHelper.SELECTED_THEME, SelectedTheme)
                && !await _settingsRepository.SaveSettingIfChanged<bool>(SettingsHelper.DARK_MODE, DarkMode)
                && !await _settingsRepository.SaveSettingIfChanged<bool>(SettingsHelper.HIGHT_CONTRAST, HighContrast)
                && !await _settingsRepository.SaveSettingIfChanged<bool>(SettingsHelper.USE_OS_THEME, UseOsTheme))
            {
                return;
            }

            await _themeSettingsManager.ApplyCurrentThemeSettings();
        }

        [RelayCommand]
        private void Close()
        {
            
        }
    }
}
