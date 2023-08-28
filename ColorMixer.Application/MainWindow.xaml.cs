using ColorMixer.Application.Services;
using MahApps.Metro.Controls;
using System.Linq;
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
                    if (TopItemsColletion.Contains(oldItem))
                    {
                        SelectedHamburgerIndex = -1;
                    }
                    else if (BottomItemsColletion.Contains(oldItem))
                    {
                        SelectedOptionsHamburgerIndex = -1;
                    }
                }
                else 
                {
                    HamburgerMenuControl.Content = newItem.Tag;
                }
            };
        }

        private void HamburgerMenu_OnItemInvoked(object sender, HamburgerMenuItemInvokedEventArgs e)
        {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            _viewManager.SwitchViewRequested((HamburgerMenuIconItem)e.InvokedItem);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        }


        public int SelectedHamburgerIndex
        {
            get { return (int)GetValue(SelectedHamburgerIndexProperty); }
            set { SetValue(SelectedHamburgerIndexProperty, value); }
        }

        public static readonly DependencyProperty SelectedHamburgerIndexProperty =
            DependencyProperty.Register(nameof(SelectedHamburgerIndex), typeof(int), typeof(MainWindow), new PropertyMetadata(-1));

        public int SelectedOptionsHamburgerIndex
        {
            get { return (int)GetValue(SelectedOptionsHamburgerIndexProperty); }
            set { SetValue(SelectedOptionsHamburgerIndexProperty, value); }
        }

        public static readonly DependencyProperty SelectedOptionsHamburgerIndexProperty =
            DependencyProperty.Register(nameof(SelectedOptionsHamburgerIndex), typeof(int), typeof(MainWindow), new PropertyMetadata(-1));

    }
}
