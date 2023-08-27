namespace ColorMixer.Contracts.Helpers
{
    public static class SettingsHelper
    {
        /// <summary>
        /// Use OS default theme in app's GUI. 
        /// </summary>
        public const string USE_OS_THEME = "USE_OS_THEME";
        /// <summary>
        /// In case USE_OS_THEME is false - SELECTED_THEME theme is used.
        /// </summary>
        public const string SELECTED_THEME = "SELECTED_THEME";
        /// <summary>
        /// In case USE_OS_THEME is false - apply value of HIGHT_CONTRAST (bool) to SELECTED_THEME.
        /// </summary>
        public const string HIGHT_CONTRAST = "HIGHT_CONTRAST";
        /// <summary>
        /// In case USE_OS_THEME is false - apply value of DARK_MODE (bool) to SELECTED_THEME.
        /// </summary>
        public const string DARK_MODE = "DARK_MODE";
        /// <summary>
        /// UiCulture to use in app. By default OS culture is used.
        /// </summary>
        public const string SELECTED_UI_CULTURE = "SELECTED_UI_CULTURE";
    }
}
