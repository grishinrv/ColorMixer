using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ColorMixer.Application.Controls
{
    /// <summary>
    /// Interaction logic for ColorNodeControl.xaml
    /// </summary>
    public partial class ColorNodeControl : UserControl
    {
        public ColorNodeControl()
        {
            InitializeComponent();
        }

        public Color Color
        {
            get { return (Color)GetValue(ColorProperty); }
            set { SetValue(ColorProperty, value); }
        }

        public static readonly DependencyProperty ColorProperty =
            DependencyProperty.Register(
                nameof(Color), 
                typeof(Color), 
                typeof(ColorNodeControl), 
                new PropertyMetadata(Colors.White));
    }
}
