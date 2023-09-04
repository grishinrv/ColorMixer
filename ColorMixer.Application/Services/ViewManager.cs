using ColorMixer.Application.Models;
using ColorMixer.Application.ViewModels;
using CommunityToolkit.Mvvm.ComponentModel;
using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ColorMixer.Application.Services
{
    public sealed class ViewManager : IViewManager
    {
        private readonly Stack<HamburgerMenuItem> _containerStack = new Stack<HamburgerMenuItem>();
        private readonly Stack<ObservableObject> _viewModelStack = new Stack<ObservableObject>();
        private ViewModelResolver _viewModelResolver;
        public event HamburgerContentSwitchDelegate OnCurrentViewChanged;

        public ViewManager(ViewModelResolver viewModelResolver)
        {
            _viewModelResolver = viewModelResolver;
        }

        public async ValueTask SwitchViewRequested(HamburgerMenuItem viewContainer)
        {
            try
            {
                Control view = (Control)viewContainer.Tag;
                if (!_containerStack.Contains(viewContainer))
                {
                    ObservableObject topViewModel = _viewModelResolver.Invoke(view);
                    if (topViewModel is IViewModelInitializable initializable)
                    {
                        await initializable.OnFirstOpen();
                    }
                    _containerStack.Push(viewContainer);
                    _viewModelStack.Push((ObservableObject)(view.DataContext));
                }

                _containerStack.TryPeek(out HamburgerMenuItem? previous);
                OnCurrentViewChanged?.Invoke(viewContainer, previous);
            }
            catch (Exception ex)
            {
                // todo log
                throw;
            }
        }

        public async ValueTask Close(ObservableObject toClose)
        {
            ObservableObject topCheck = _viewModelStack.Pop();
            if (topCheck != toClose)
                throw new InvalidOperationException("Close called not from UI");

            HamburgerMenuItem closingContainer = _containerStack.Pop();
            HamburgerMenuItem? previousContainer = null;

            if (_viewModelStack.TryPop(out ObservableObject? _))
            {
                previousContainer = _containerStack.Pop();
            }

            OnCurrentViewChanged?.Invoke(previousContainer, closingContainer);
            if (closingContainer.Tag is IDisposable disposableView)
            {
                disposableView.Dispose();
            }
            if (toClose is IDisposable disposableViewModel)
            {
                disposableViewModel.Dispose();
            }
            else if (toClose is IAsyncDisposable asyncDisposableViewModel)
            { 
                await asyncDisposableViewModel.DisposeAsync();
            }
        }
    }
}
