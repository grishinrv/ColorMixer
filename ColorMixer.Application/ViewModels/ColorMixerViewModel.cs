using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows.Media;

namespace ColorMixer.Application.ViewModels
{
    public sealed class ColorMixerViewModel : ObservableObject
    {
        public ColorMixerViewModel()
        {
            Color color = new Color();
        }
    }
}
