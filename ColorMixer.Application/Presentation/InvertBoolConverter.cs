using System;
using System.Globalization;
using MahApps.Metro.ValueBoxes;
using System.Windows.Data;
using System.Windows.Markup;

namespace ColorMixer.Application.Presentation
{
    [ValueConversion(typeof(bool), typeof(bool))]
    public class InvertBoolConverter : MarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null && value is bool booleanVal)
            {
                if (booleanVal)
                    return BooleanBoxes.FalseBox;
            }

            return BooleanBoxes.TrueBox;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Convert(value, targetType, parameter, culture);
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}
