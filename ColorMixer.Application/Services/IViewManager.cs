using CommunityToolkit.Mvvm.ComponentModel;
using System.Threading.Tasks;

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
        ValueTask Close(ObservableObject viewModel);
    }
}