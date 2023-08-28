using ColorMixer.Application.Services;
using ColorMixer.Application.ViewModels;
using CommunityToolkit.Mvvm.ComponentModel;
using MahApps.Metro.Controls;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ColorMixer.Application
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public sealed partial class MainWindow : MetroWindow
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ViewModelResolver _viewModelResolver;
        public MainWindow(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _viewModelResolver = serviceProvider.GetRequiredService<ViewModelResolver>();
            InitializeComponent();
        }


        private async void HamburgerMenu_OnItemInvoked(object sender, HamburgerMenuItemInvokedEventArgs e)
        {
            try
            {
                Control view = (Control)((HamburgerMenuItem)e.InvokedItem).Tag;
                ObservableObject viewModel = _viewModelResolver.Invoke(view);
                if (viewModel is IViewModelInitializable initializable)
                {
                    await initializable.OnFirstOpen();
                }

                HamburgerMenuControl.Content = e.InvokedItem;
            }
            catch (Exception ex)
            {
                // todo log
                Environment.Exit(1);
            }
        }
    }
}
