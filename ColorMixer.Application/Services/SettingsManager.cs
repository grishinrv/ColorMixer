using ColorMixer.Contracts.Helpers;
using ColorMixer.Contracts.Services;
using ControlzEx.Theming;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ColorMixer.Application.Services
{
    public sealed class SettingsManager
    {
        private readonly ISettingsRepository _settingsRepository;
        public SettingsManager(ISettingsRepository settingsRepository)
        {
            _settingsRepository = settingsRepository;
        }

        public async Task ApplyCurrentThemeSettings()
        {
            bool syncWithOs = await _settingsRepository.GetSetting<bool>(SettingsHelper.USE_OS_THEME);
            if (syncWithOs)
                ThemeManager.Current.ThemeSyncMode = ThemeSyncMode.SyncAll;
            else
            {
                string themeName = await _settingsRepository.GetSetting<string>(SettingsHelper.SELECTED_THEME);
                bool isDarkMode = await _settingsRepository.GetSetting<bool>(SettingsHelper.DARK_MODE);
                bool isHighContrast = await _settingsRepository.GetSetting<bool>(SettingsHelper.HIGHT_CONTRAST);
                themeName = isDarkMode ? $"{themeName}.Dark" : $"{themeName}.Light";
                ThemeManager.Current.ChangeTheme(App.Current, themeName, isHighContrast);
            }

            ThemeManager.Current.SyncTheme();
        }
    }
}
