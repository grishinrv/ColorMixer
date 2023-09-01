using ColorMixer.Application.Models;
using ColorMixer.Contracts.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Windows.Media;

namespace ColorMixer.Application.ViewModels
{
    public sealed partial class ColorMixerViewModel : ObservableObject
    {
        public ColorMixerViewModel()
        {
            ColorNodes.Add(Selected);
        }

        public ObservableCollection<ColorNodeViewModel> ColorNodes { get; } = new ObservableCollection<ColorNodeViewModel>();

        [ObservableProperty]
        private ColorNodeViewModel _selected = new ColorNodeViewModel{ Left = 100, Top = 100 };
        [ObservableProperty]
        private ColorNodeViewModel? _target;
        [ObservableProperty]
        private MixingType _selectedMixingType;
        private Color _pickedColor;
        public Color PickedColor
        {
            get => _pickedColor;
            set
            {
                SetProperty(ref _pickedColor, value);
                Selected.Color = value;
            }
        }

        [RelayCommand]
        public void Mix(ColorMixingEventArgs mixingEventArgs)
        {
            if (mixingEventArgs.Left == mixingEventArgs.Right)
                return;

            ColorNodeViewModel mixingResult = ColorNodeViewModel.Mix(
                mixingEventArgs.Left, mixingEventArgs.Right, mixingEventArgs.Operation);

            ColorNodes.Add(mixingResult);
        }

        [RelayCommand]
        public void AddNewParentlessColorNode()
        {
            ColorNodes.Add(new ColorNodeViewModel { Left = 200, Top = 200, Color = PickedColor});
        }
    }
}
