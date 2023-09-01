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
        public double PositionTop
        {
            get { return (double)GetValue(PositionTopProperty); }
            set { SetValue(PositionTopProperty, value); }
        }
        public double PositionLeft
        {
            get { return (double)GetValue(PositionLeftProperty); }
            set { SetValue(PositionLeftProperty, value); }
        }

        public static readonly DependencyProperty ColorProperty =
            DependencyProperty.Register(
                nameof(Color), 
                typeof(Color), 
                typeof(ColorNodeControl), 
                new PropertyMetadata(Colors.White));

        public static readonly DependencyProperty PositionTopProperty =
            DependencyProperty.Register(
                nameof(PositionTop),
                typeof(double),
                typeof(ColorNodeControl),
                new PropertyMetadata(0.0));

        public static readonly DependencyProperty PositionLeftProperty =
            DependencyProperty.Register(
                nameof(PositionLeft), 
                typeof(double), 
                typeof(ColorNodeControl), 
                new PropertyMetadata(0.0));
    }
}
