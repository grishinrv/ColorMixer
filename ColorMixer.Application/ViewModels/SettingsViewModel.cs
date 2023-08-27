using ColorMixer.Contracts.Helpers;
using ColorMixer.Contracts.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ColorMixer.Application.ViewModels
{
    public sealed partial class SettingsViewModel : ObservableObject
    {
        private readonly ISettingsRepository _settingsRepository;

#pragma warning disable CS8618
        public SettingsViewModel(ISettingsRepository settingsRepository)
#pragma warning restore CS8618
        {
            _settingsRepository = settingsRepository;
        }

        public async Task OnFirstOpen()
        {
            SelectedTheme = await _settingsRepository.GetSetting<string>(SettingsHelper.SELECTED_THEME);
            DarkMode = await _settingsRepository.GetSetting<bool>(SettingsHelper.DARK_MODE);
            HighContrast = await _settingsRepository.GetSetting<bool>(SettingsHelper.HIGHT_CONTRAST);
            UseOsTheme = await _settingsRepository.GetSetting<bool>(SettingsHelper.USE_OS_THEME);
            SelectedUiCulture = await _settingsRepository.GetSetting<string>(SettingsHelper.SELECTED_UI_CULTURE);
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
        private string _selectedUiCulture;

        /// <summary>
        /// MahApps Metro themes list: 
        /// https://mahapps.com/docs/themes/usage
        /// </summary>
        public List<string> Themes { get; } = new List<string>()
        {
            "Red", "Green", "Blue", "Purple", "Orange", "Lime", "Emerald", "Teal", "Cyan", "Cobalt", "Indigo",
            "Violet", "Pink", "Magenta", "Crimson", "Amber", "Yellow", "Brown", "Olive", "Steel", "Mauve", "Taupe", "Sienna"
        };

        /// <summary>
        /// Supported cultures: 
        /// 
        /// </summary>
        public List<string> Cultures { get; } = new List<string>()
        {
            "ru", "de", "en"
        };

        [RelayCommand]
        private async Task ApplyChanges()
        {
            await _settingsRepository.SaveSettingIfChanged<string>(SettingsHelper.SELECTED_THEME, SelectedTheme);
            await _settingsRepository.SaveSettingIfChanged<bool>(SettingsHelper.DARK_MODE, DarkMode);
            await _settingsRepository.SaveSettingIfChanged<bool>(SettingsHelper.HIGHT_CONTRAST, HighContrast);
            await _settingsRepository.SaveSettingIfChanged<bool>(SettingsHelper.USE_OS_THEME, UseOsTheme);
            await _settingsRepository.SaveSettingIfChanged<string>(SettingsHelper.SELECTED_UI_CULTURE, SelectedUiCulture);
        }

        [RelayCommand]
        private void Close()
        {
            
        }
    }
}
