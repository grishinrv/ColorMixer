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
        private readonly IViewManager _viewManager;

#pragma warning disable CS8618
        public SettingsViewModel(
            ISettingsRepository settingsRepository, 
            ThemeSettingsManager themeSettingsManager,
            IViewManager viewManager)
#pragma warning restore CS8618
        {
            _settingsRepository = settingsRepository;
            _themeSettingsManager = themeSettingsManager;
            _viewManager = viewManager;
        }

        public async Task OnFirstOpen()
        {
            SelectedTheme = await _settingsRepository.GetSetting<string>(SettingsHelper.SELECTED_THEME);
            DarkMode = await _settingsRepository.GetSetting<bool>(SettingsHelper.DARK_MODE);
            UseOsTheme = await _settingsRepository.GetSetting<bool>(SettingsHelper.USE_OS_THEME);
            string currentUiCulture = await _settingsRepository.GetSetting<string>(SettingsHelper.SELECTED_UI_CULTURE);
            SelectedCultureIndex = Array.IndexOf(Cultures, Cultures.First(x => x.Name == currentUiCulture));
            ActualizeSelectedThemeIndex();
        }

        private void ActualizeSelectedThemeIndex()
        {
            for (int i = 0; i < Themes.Count; i++)
            {
                if (string.Equals(Themes.ElementAt(i), SelectedTheme))
                {
                    SelectedThemeIndex = i;
                    break;
                }
            }
        }

        [ObservableProperty]
        private string _selectedTheme;

        [ObservableProperty]
        private bool _darkMode;

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

        private int _selectedThemeIndex;
        public int SelectedThemeIndex
        {
            get => _selectedThemeIndex;
            set
            {
                SetProperty(ref _selectedThemeIndex, value);
                SelectedTheme = Themes[value];
            }
        }


        [RelayCommand]
        private async Task ApplyChanges()
        {
            await ApplyThemeSettingsIfNeeded();
            await _settingsRepository.SaveSettingIfChanged<string>(SettingsHelper.SELECTED_UI_CULTURE, SelectedUiCulture.Name);
            await Close();
        }


        private async Task ApplyThemeSettingsIfNeeded()
        {
            if (!await _settingsRepository.SaveSettingIfChanged<string>(SettingsHelper.SELECTED_THEME, SelectedTheme)
                && !await _settingsRepository.SaveSettingIfChanged<bool>(SettingsHelper.DARK_MODE, DarkMode)
                && !await _settingsRepository.SaveSettingIfChanged<bool>(SettingsHelper.USE_OS_THEME, UseOsTheme))
            {
                return;
            }

            await _themeSettingsManager.ApplyCurrentThemeSettings();
        }

        [RelayCommand]
        private async Task Close()
        {
            await _viewManager.Close(this);
        }
    }
}
