using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows.Controls;
using ColorMixer.Contracts.Exceptions;

namespace ColorMixer.Application.Services
{
    /// <summary>
    /// Responsible for resolving viewModel for provided view and setting it's DataContext.
    /// </summary>
    /// <param name="view">A view instance.</param>
    /// <returns>ViewModel</returns>
    /// <throws><see cref="FailedResolveInstanceException"/></throws>
    public delegate ObservableObject ViewModelResolver(Control view);
}
