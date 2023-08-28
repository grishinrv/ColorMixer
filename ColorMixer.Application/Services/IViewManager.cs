using CommunityToolkit.Mvvm.ComponentModel;

namespace ColorMixer.Application.Services
{
    /// <summary>
    /// Managers current view, shown by Hamburger Menu.
    /// </summary>
    public interface IViewManager
    {
        /// <summary>
        /// Closes current view.
        /// </summary>
        /// <param name="viewModel">View model, requesting close.</param>
        void Close(ObservableObject viewModel);
    }
}