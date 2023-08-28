using ColorMixer.Application.ViewModels;
using CommunityToolkit.Mvvm.ComponentModel;
using MahApps.Metro.Controls;
using System;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ColorMixer.Application.Services
{
    public sealed class ViewManager : IViewManager
    {
        private HamburgerMenuItem? _currentViewContainer;
        private ObservableObject? _currentViewModel;
        private ViewModelResolver _viewModelResolver;

        public event Action<HamburgerMenuItem?> OnCurrentViewChanged;

        public ViewManager(ViewModelResolver viewModelResolver)
        {
            _viewModelResolver = viewModelResolver;
        }


        public async Task SwitchViewRequested(HamburgerMenuItem viewContainer)
        {
            try
            {
                Control view = (Control)viewContainer.Tag;
                _currentViewModel = _viewModelResolver.Invoke(view);
                if (_currentViewModel is IViewModelInitializable initializable)
                {
                    await initializable.OnFirstOpen();
                }
                _currentViewContainer = viewContainer;
                OnCurrentViewChanged?.Invoke(viewContainer);
            }
            catch (Exception ex)
            {
                // todo log
                Environment.Exit(1);
            }
        }

        public void Close(ObservableObject viewModel)
        {
            if (_currentViewModel == viewModel)
            {
                _currentViewModel = null;
                _currentViewContainer = null;
                OnCurrentViewChanged?.Invoke(_currentViewContainer);
            }
        }
    }
}
