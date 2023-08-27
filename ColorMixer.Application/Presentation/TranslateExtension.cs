using ColorMixer.Contracts.Services;
using System;
using System.Windows.Data;
using System.Windows.Markup;

namespace ColorMixer.Application.Presentation
{
    /// <summary>
    /// Localizaing extension. XAML exapmle:
    /// Text="{helpers:Translate Key=SomeKey}"
    /// </summary>
    public sealed class TranslateExtension : MarkupExtension
    {
        public string? Key { get; set; }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return new Binding
            {
                Mode = BindingMode.OneWay,
                Path = new System.Windows.PropertyPath($"[{Key}]"),
                Source = LocalizationService.Instance
            };
        }
    }
}
