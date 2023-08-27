using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Resources;

namespace ColorMixer.Contracts.Services
{
    public sealed class LocalizationService : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        private ResourceManager _resourceManager = new ResourceManager("ColorMixer.Contracts.Localization.Translation", Assembly.GetExecutingAssembly());

        public static LocalizationService Instance { get; } = new LocalizationService();

        public string this[string key] => _resourceManager.GetString(key, CultureInfo.CurrentUICulture) ?? string.Empty;

        public void SetCulture(CultureInfo culture)
        {
            CultureInfo.CurrentUICulture = culture;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null));
        }
    }
}
