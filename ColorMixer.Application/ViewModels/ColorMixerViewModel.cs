using ColorMixer.Application.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace ColorMixer.Application.ViewModels
{
    public sealed class ColorMixerViewModel : ObservableObject
    {
        public ColorMixerViewModel()
        {
        }

        public ObservableCollection<ColorNodeViewModel> ColorNodes { get; } = new ObservableCollection<ColorNodeViewModel>()
        { 
            new ColorNodeViewModel{ Left = 100, Top = 100 }
        };

        public void Mix(object sender, ColorMixingEventArgs mixingEventArgs)
        {
            if (mixingEventArgs.Left == mixingEventArgs.Right)
                return;

            ColorNodeViewModel mixingResult = ColorNodeViewModel.Mix(
                mixingEventArgs.Left, mixingEventArgs.Right, mixingEventArgs.Operation);

            ColorNodes.Add(mixingResult);
        }

    }
}
