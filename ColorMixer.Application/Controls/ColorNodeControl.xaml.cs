using ColorMixer.Application.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
