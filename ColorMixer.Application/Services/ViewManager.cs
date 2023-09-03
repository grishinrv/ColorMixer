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
        private HamburgerMenuItem? _previousViewContainer;
        private HamburgerMenuItem? _currentViewContainer;
        private ObservableObject? _currentViewModel;
        private ViewModelResolver _viewModelResolver;
        // todo named delegate with event args (named params)
        public event Action<HamburgerMenuItem?, HamburgerMenuItem?> OnCurrentViewChanged;

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
                HamburgerMenuItem? _previousViewContainer = _currentViewContainer;
                _currentViewContainer = viewContainer;
                OnCurrentViewChanged?.Invoke(viewContainer, _previousViewContainer);
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
                HamburgerMenuItem? _previousViewContainer = _currentViewContainer;
                _currentViewModel = null;
                _currentViewContainer = null;
                OnCurrentViewChanged?.Invoke(_currentViewContainer, _previousViewContainer);
            }
        }
    }
}
