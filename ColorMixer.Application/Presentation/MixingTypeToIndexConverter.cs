using ColorMixer.Contracts.Models;
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace ColorMixer.Application.Presentation
{
    [ValueConversion(typeof(MixingType), typeof(int))]
    public sealed class MixingTypeToIndexConverter : MarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is MixingType mixing)
            {
                int index = (int)mixing;
                return --index;
            }
            throw new ArgumentException(nameof(value));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int mixingIndex)
            {
                MixingType result = (MixingType)((byte)++mixingIndex);
                return result;
            }

            throw new ArgumentException(nameof(value));
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}
