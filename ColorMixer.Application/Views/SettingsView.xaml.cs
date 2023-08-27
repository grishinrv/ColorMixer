using ColorMixer.Contracts.Services;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace ColorMixer.Application.Views
{
    /// <summary>
    /// Interaction logic for SettingsView.xaml
    /// </summary>
    public partial class SettingsView : UserControl
    {
        public SettingsView()
        {
            InitializeComponent();
            Binding binding = new Binding("[settings]");
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
                typeof(SettingsView), 
                new PropertyMetadata(string.Empty));

    }

}
