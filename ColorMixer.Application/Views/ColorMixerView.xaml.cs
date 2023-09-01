using ColorMixer.Contracts.Services;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using ColorMixer.Application.Controls;
using ColorMixer.Contracts.Models;
using ColorMixer.Application.Presentation;
using CommunityToolkit.Mvvm.Input;
using ColorMixer.Application.Models;

namespace ColorMixer.Application.Views
{
    /// <summary>
    /// Interaction logic for ColorMixerView.xaml
    /// </summary>
    public partial class ColorMixerView : UserControl
    {
        private bool _isDown;
        private bool _isDragging;
        private ColorNodeControl? _originalElement;
        private double _originalLeft;
        private double _originalTop;
        private CircleAdorner _overlayElement;
        private Point _startPoint;
        private Canvas _mixingCanvas;

        #region Constructor
        public ColorMixerView()
        {
            InitializeComponent();
            Binding titleBinding = new Binding("[color_set]");
            titleBinding.Mode = BindingMode.OneWay;
            titleBinding.Source = LocalizationService.Instance;
            BindingOperations.SetBinding(this, TitleProperty, titleBinding);

            Binding mixgommandBinding = new Binding("MixCommand");
            mixgommandBinding.Mode = BindingMode.OneWay;
            mixgommandBinding.Source = DataContext;
            BindingOperations.SetBinding(this, MixColorsCommandProperty, mixgommandBinding);

            Binding selectedNodeBinding = new Binding("Selected");
            selectedNodeBinding.Mode = BindingMode.TwoWay;
            selectedNodeBinding.Source = DataContext;
            BindingOperations.SetBinding(this, SelectedColorNodeProperty, selectedNodeBinding);

            Binding targetNodeBinding = new Binding("Target");
            targetNodeBinding.Mode = BindingMode.TwoWay;
            targetNodeBinding.Source = DataContext;
            BindingOperations.SetBinding(this, TargetColorNodeProperty, targetNodeBinding);

            Binding selectedMixingTypeBinding = new Binding("SelectedMixingType");
            selectedMixingTypeBinding.Mode = BindingMode.OneWay;
            selectedMixingTypeBinding.Source = DataContext;
            BindingOperations.SetBinding(this, TargetColorNodeProperty, selectedMixingTypeBinding);
        }
        #endregion
        #region Dependecy properties
        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        public RelayCommand<ColorMixingEventArgs> MixColorsCommand
        {
            get { return (RelayCommand<ColorMixingEventArgs>)GetValue(MixColorsCommandProperty); }
            set { SetValue(MixColorsCommandProperty, value); }
        }

        public IColorNode SelectedColorNode
        {
            get { return (IColorNode)GetValue(SelectedColorNodeProperty); }
            set { SetValue(SelectedColorNodeProperty, value); }
        }
        public IColorNode TargetColorNode
        {
            get { return (IColorNode)GetValue(TargetColorNodeProperty); }
            set { SetValue(TargetColorNodeProperty, value); }
        }

        public MixingType SelectedMixingType
        {
            get { return (MixingType)GetValue(SelectedMixingTypeProperty); }
            set { SetValue(SelectedMixingTypeProperty, value); }
        }

        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register(
                nameof(Title),
                typeof(string),
                typeof(ColorMixerView),
                new PropertyMetadata("Defaul color set name"));

        public static readonly DependencyProperty MixColorsCommandProperty =
            DependencyProperty.Register(
                nameof(MixColorsCommand), 
                typeof(RelayCommand<ColorMixingEventArgs>), 
                typeof(ColorMixerView), 
                new PropertyMetadata(null));

        public static readonly DependencyProperty SelectedColorNodeProperty =
            DependencyProperty.Register(
                nameof(SelectedColorNode),
                typeof(IColorNode),
                typeof(ColorMixerView),
                new PropertyMetadata(null));

        public static readonly DependencyProperty TargetColorNodeProperty =
            DependencyProperty.Register(
                nameof(TargetColorNode),
                typeof(IColorNode), 
                typeof(ColorMixerView), 
                new PropertyMetadata(null));

        public static readonly DependencyProperty SelectedMixingTypeProperty =
            DependencyProperty.Register(
                nameof(SelectedMixingType), 
                typeof(MixingType), 
                typeof(ColorMixerView), 
                new PropertyMetadata(MixingType.Undefined));

        #endregion

        public void OnLoaded(object sender, RoutedEventArgs e)
        {
            _mixingCanvas = ColorNodesItemsPresenterControl.GetItemsPanel<Canvas>()!;
            if (_mixingCanvas == null) 
            {
                throw new InvalidOperationException("Not possible to perform mixing without canvas");
            }

            _mixingCanvas.PreviewMouseLeftButtonDown += MixingCanvas_PreviewMouseLeftButtonDown;
            _mixingCanvas.PreviewMouseMove += MixingCanvas_PreviewMouseMove;
            _mixingCanvas.PreviewMouseLeftButtonUp += MixingCanvas_PreviewMouseLeftButtonUp;
            PreviewKeyDown += ColorMixer_PreviewKeyDown;
        }


        private void MixingCanvas_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.Source is ColorNodeControl originalElement)
            {
                _originalElement = originalElement;
                _isDown = true;
                _startPoint = e.GetPosition(_mixingCanvas);
                _mixingCanvas.CaptureMouse();
                e.Handled = true;
            }
        }

        private void ColorMixer_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape && _isDragging)
            {
                DragFinished(true);
            }
        }

        private void MixingCanvas_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (_isDown && e.Source == _mixingCanvas)
            {
                ColorNodeControl? target = _mixingCanvas.FindChildByTypeAndPoint<ColorNodeControl>(e.GetPosition(App.Current.MainWindow));
                if (target != null && target.DataContext is IColorNode targetContext)
                {
                    TargetColorNode = targetContext;
                    MixColorsCommand.Execute(
                        new ColorMixingEventArgs(SelectedColorNode, TargetColorNode, SelectedMixingType));
                    DragFinished(true);
                }
                else
                {
                    DragFinished(false);
                }
                _isDragging = false;
                _isDown = false;
                e.Handled = true;
            }
        }


        private void DragFinished(bool cancelled)
        {
            Mouse.Capture(null);
            if (_isDragging)
            {
                AdornerLayer.GetAdornerLayer(_overlayElement.AdornedElement).Remove(_overlayElement);

                if (cancelled == false)
                {
                    Canvas.SetTop(_originalElement, _originalTop + _overlayElement.TopOffset);
                    Canvas.SetLeft(_originalElement, _originalLeft + _overlayElement.LeftOffset);
                }
                _overlayElement = null;
            }
        }
        private void MixingCanvas_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (_isDown)
            {
                if ((_isDragging == false) &&
                    ((Math.Abs(e.GetPosition(_mixingCanvas).X - _startPoint.X) >
                      SystemParameters.MinimumHorizontalDragDistance) ||
                     (Math.Abs(e.GetPosition(_mixingCanvas).Y - _startPoint.Y) >
                      SystemParameters.MinimumVerticalDragDistance)))
                {
                    DragStarted();
                }

                if (_isDragging)
                    DragMoved();
            }
        }

        private void DragStarted()
        {
            _isDragging = true;
            _originalLeft = Canvas.GetLeft(_originalElement);
            _originalTop = Canvas.GetTop(_originalElement);
            _overlayElement = new CircleAdorner(_originalElement);
            AdornerLayer layer = AdornerLayer.GetAdornerLayer(_originalElement);
            layer.Add(_overlayElement);
        }

        private void DragMoved()
        {
            Point currentPosition = Mouse.GetPosition(_mixingCanvas);

            _overlayElement.LeftOffset = currentPosition.X - _startPoint.X;
            _overlayElement.TopOffset = currentPosition.Y - _startPoint.Y;
        }
    }
}
