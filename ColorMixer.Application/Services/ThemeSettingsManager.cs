using ColorMixer.Contracts.Helpers;
using ColorMixer.Contracts.Services;
using ControlzEx.Theming;
using System.Threading.Tasks;

namespace ColorMixer.Application.Services
{
    public sealed class ThemeSettingsManager
    {
        private readonly ISettingsRepository _settingsRepository;
        public ThemeSettingsManager(ISettingsRepository settingsRepository)
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
                themeName = isDarkMode ? $"Dark.{themeName}" : $"Light.{themeName}";
                ThemeManager.Current.ChangeTheme(App.Current, themeName, isHighContrast);
            }

            ThemeManager.Current.SyncTheme();
        }
    }
}
