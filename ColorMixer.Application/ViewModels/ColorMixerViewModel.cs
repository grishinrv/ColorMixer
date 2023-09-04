using ColorMixer.Application.Models;
using ColorMixer.Application.Services;
using ColorMixer.Contracts.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
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

        public ObservableCollection<IColorNode> ColorNodes { get; } = new ObservableCollection<IColorNode>();

        [ObservableProperty]
        private ColorNodeViewModel _selected = new ColorNodeViewModel{ Left = 300, Top = 300 };
        [ObservableProperty]
        private ColorNodeViewModel? _target;
        [ObservableProperty]
        private ColorNodeViewModel? _mixingResult;
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
        private void Mix()
        {
            MixingResult = ColorNodeViewModel.Mix(Selected, Target!, SelectedMixingType);
            ColorNodes.Add(MixingResult);
        }

        [RelayCommand]
        private void AddNewParentlessColorNode()
        {
            ColorNodes.Add(new ColorNodeViewModel { Left = 200, Top = 200, Color = PickedColor});
        }

        [RelayCommand]
        private async Task Close()
        {
            await _viewManager.Close(this);
        }
    }
}
