using ColorMixer.Contracts.Services;
using System.Windows.Data;

namespace ColorMixer.Application.Presentation
{
    /// <summary>
    /// Localizaing extension. XAML exapmle:
    /// Text="{helpers:Translate Key=SomeKey}"
    /// </summary>
    public sealed class TranslateExtension : Binding
    {
        public TranslateExtension()
        {
            Initialize();
        }

        public TranslateExtension(string key)
            : base($"[{key}]")
        {
            Initialize();
        }

        private void Initialize()
        {
            this.Source = LocalizationService.Instance;
            this.Mode = BindingMode.OneWay;
        }
    }
}
