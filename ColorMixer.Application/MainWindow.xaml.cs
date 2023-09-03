using ColorMixer.Application.Services;
using MahApps.Metro.Controls;
using System.Windows;

namespace ColorMixer.Application
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public sealed partial class MainWindow : MetroWindow
    {
        private readonly ViewManager _viewManager;
        public MainWindow(ViewManager viewManager)
        {
            _viewManager = viewManager;
            InitializeComponent();
            _viewManager.OnCurrentViewChanged += (newItem, oldItem) =>
            {
                if (newItem == null)
                {
                    SelectedHamburgerIndex = -1;
                    SelectedOptionsHamburgerIndex = -1;
                    HamburgerMenuControl.Content = null;
                }
                else 
                {
                    if (TopItemsColletion.Contains(newItem))
                        SelectedHamburgerIndex = TopItemsColletion.IndexOf(newItem);
                    else if (BottomItemsColletion.Contains(newItem))
                        SelectedOptionsHamburgerIndex = BottomItemsColletion.IndexOf(newItem);
                    HamburgerMenuControl.Content = newItem.Tag;
                }
            };
        }

        private static async void SelectedItemChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is MainWindow window && window.SelectedHamburgerIndex >= 0)
            {
                await window._viewManager.SwitchViewRequested(
                    (HamburgerMenuIconItem)(window.TopItemsColletion[window.SelectedHamburgerIndex]));
            }
        }

        private static async void SelectedOptionsItemChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is MainWindow window && window.SelectedOptionsHamburgerIndex >= 0)
            {
                await window._viewManager.SwitchViewRequested(
                    (HamburgerMenuIconItem)(window.BottomItemsColletion[window.SelectedOptionsHamburgerIndex]));
            }
        }

        public int SelectedHamburgerIndex
        {
            get { return (int)GetValue(SelectedHamburgerIndexProperty); }
            set { SetValue(SelectedHamburgerIndexProperty, value); }
        }
        public int SelectedOptionsHamburgerIndex
        {
            get { return (int)GetValue(SelectedOptionsHamburgerIndexProperty); }
            set { SetValue(SelectedOptionsHamburgerIndexProperty, value); }
        }

        public static readonly DependencyProperty SelectedHamburgerIndexProperty =
            DependencyProperty.Register(
                nameof(SelectedHamburgerIndex), 
                typeof(int), 
                typeof(MainWindow), 
                new PropertyMetadata(-1, new PropertyChangedCallback(SelectedItemChanged)));

        public static readonly DependencyProperty SelectedOptionsHamburgerIndexProperty =
            DependencyProperty.Register(
                nameof(SelectedOptionsHamburgerIndex), 
                typeof(int), 
                typeof(MainWindow), 
                new PropertyMetadata(-1, new PropertyChangedCallback(SelectedOptionsItemChanged)));
    }
}
