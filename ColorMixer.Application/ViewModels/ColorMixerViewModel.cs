using ColorMixer.Application.Services;
using ColorMixer.Contracts.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Windows.Media;

namespace ColorMixer.Application.ViewModels
{
    public sealed partial class ColorMixerViewModel : ObservableObject
    {
        private readonly IViewManager _viewManager;

        public ColorMixerViewModel(IViewManager viewManager)
        {
            _viewManager = viewManager;
            ColorNodes.Add(Selected);
            SelectedMixingType = MixingType.Subtractive;
        }

        public ObservableCollection<ColorNodeViewModel> ColorNodes { get; } = new ObservableCollection<ColorNodeViewModel>();

        [ObservableProperty]
        private ColorNodeViewModel _selected = new ColorNodeViewModel{ Left = 300, Top = 300 };
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
        public void Mix()
        {
            if (Selected == Target || Target == null)
                return;

            ColorNodeViewModel mixingResult = ColorNodeViewModel.Mix(
                Selected, Target, SelectedMixingType);

            ColorNodes.Add(mixingResult);
        }

        [RelayCommand]
        private void AddNewParentlessColorNode()
        {
            ColorNodes.Add(new ColorNodeViewModel { Left = 200, Top = 200, Color = PickedColor});
        }

        [RelayCommand]
        private void Close()
        {
            _viewManager.Close(this);
        }
    }
}
