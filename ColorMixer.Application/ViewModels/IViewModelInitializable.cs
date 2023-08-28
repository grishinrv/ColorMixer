using System.Threading.Tasks;

namespace ColorMixer.Application.ViewModels
{
    public interface IViewModelInitializable
    {
        /// <summary>
        /// Init View Model.
        /// </summary>
        public Task OnFirstOpen();
    }
}
