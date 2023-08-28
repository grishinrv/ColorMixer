using ColorMixer.Contracts.Services;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace ColorMixer.Application.Views
{
    /// <summary>
    /// Interaction logic for ColorMixerView.xaml
    /// </summary>
    public partial class ColorMixerView : UserControl
    {
        public ColorMixerView()
        {
            InitializeComponent();
            Binding binding = new Binding("[color_set]");
            binding.Mode = BindingMode.OneWay;
            binding.Source = LocalizationService.Instance;
            BindingOperations.SetBinding(this, TitleProperty, binding);
        }

        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register(
                nameof(Title),
                typeof(string),
                typeof(ColorMixerView),
                new PropertyMetadata("Defaul color set name"));
    }
}
