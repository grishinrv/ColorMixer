using ColorMixer.Contracts.Services;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using ColorMixer.Application.Controls;
using ColorMixer.Application.Presentation;
using ColorMixer.Application.Models;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Controls.Primitives;

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
        // todo - move this and show logic into ColorNodeControl
        private CircleAdorner _overlayElement;
        private Point _startPoint;
        private Canvas _mixingCanvas;

        #region Initialization
        public ColorMixerView()
        {
            InitializeComponent();
        }

        private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            Binding titleBinding = new Binding("[color_set]");
            titleBinding.Mode = BindingMode.OneWay;
            titleBinding.Source = LocalizationService.Instance;
            BindingOperations.SetBinding(this, TitleProperty, titleBinding);

            Binding mixCommandBinding = new Binding("MixCommand");
            mixCommandBinding.Mode = BindingMode.OneWay;
            mixCommandBinding.Source = DataContext;
            BindingOperations.SetBinding(this, MixColorsCommandProperty, mixCommandBinding);

            Binding selectedNodeBinding = new Binding("Selected");
            selectedNodeBinding.Mode = BindingMode.TwoWay;
            selectedNodeBinding.Source = DataContext;
            BindingOperations.SetBinding(this, SelectedColorNodeProperty, selectedNodeBinding);

            Binding targetNodeBinding = new Binding("Target");
            targetNodeBinding.Mode = BindingMode.TwoWay;
            targetNodeBinding.Source = DataContext;
            BindingOperations.SetBinding(this, TargetColorNodeProperty, targetNodeBinding);

            Binding mixingResult = new Binding("MixingResult");
            mixingResult.Mode = BindingMode.OneWay;
            mixingResult.Source = DataContext;
            BindingOperations.SetBinding(this, MixingResultColorNodeProperty, mixingResult);
        }

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

        #endregion
        #region Dependecy properties
        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        public ICommand MixColorsCommand
        {
            get { return (ICommand)GetValue(MixColorsCommandProperty); }
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
        public IColorNode MixingResultColorNode
        {
            get { return (IColorNode)GetValue(MixingResultColorNodeProperty); }
            set { SetValue(MixingResultColorNodeProperty, value); }
        }

        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register(
                nameof(Title),
                typeof(string),
                typeof(ColorMixerView),
                new PropertyMetadata("Default color set name"));

        public static readonly DependencyProperty MixColorsCommandProperty =
            DependencyProperty.Register(
                nameof(MixColorsCommand), 
                typeof(ICommand), 
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

        public static readonly DependencyProperty MixingResultColorNodeProperty =
            DependencyProperty.Register(
                nameof(MixingResultColorNode),
                typeof(IColorNode),
                typeof(ColorMixerView),
                new PropertyMetadata(null));

        #endregion

        private void MixingCanvas_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.Source is ColorNodeControl originalElement)
            {
                _originalElement = originalElement;
                SelectedColorNode = (IColorNode)_originalElement.DataContext;
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
            if (_isDown && _isDragging && e.Source == _mixingCanvas)
            {
                ColorNodeControl? target = _mixingCanvas.FindChildByTypeAndPoint<ColorNodeControl>(e.GetPosition(App.Current.MainWindow));
                if (target != null && target.DataContext is IColorNode targetContext)
                {
                    TargetColorNode = targetContext;
                    if (TargetColorNode != SelectedColorNode)
                    {
                        MixColorsCommand.Execute(null);
                        CreateRelationLines();
                    }
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
                    _originalElement!.PositionTop += _overlayElement.TopOffset;
                    _originalElement!.PositionLeft += _overlayElement.LeftOffset;
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
            _overlayElement = new CircleAdorner(_originalElement!);
            AdornerLayer layer = AdornerLayer.GetAdornerLayer(_originalElement);
            layer.Add(_overlayElement);
        }

        private void DragMoved()
        {
            Point currentPosition = Mouse.GetPosition(_mixingCanvas);

            _overlayElement.LeftOffset = currentPosition.X - _startPoint.X;
            _overlayElement.TopOffset = currentPosition.Y - _startPoint.Y;
        }

        private void CreateRelationLines()
        {
            CreateRelationLine(SelectedColorNode, MixingResultColorNode);
            CreateRelationLine(TargetColorNode, MixingResultColorNode);
        }

        private void CreateRelationLine(IColorNode from, IColorNode to)
        {
            Line line = new Line() 
            { 
                Stroke = new SolidColorBrush(Colors.Gray),
                StrokeThickness = 1.35
            };

            Binding beginXbinding = new Binding("Top");
            beginXbinding.Mode = BindingMode.OneWay;
            beginXbinding.Source = from;
            BindingOperations.SetBinding(line, Line.X1Property, beginXbinding);

            Binding beginYbinding = new Binding("Left");
            beginYbinding.Mode = BindingMode.OneWay;
            beginYbinding.Source = from;
            BindingOperations.SetBinding(line, Line.Y1Property, beginXbinding);

            Binding endXbinding = new Binding("Top");
            endXbinding.Mode = BindingMode.OneWay;
            endXbinding.Source = to;
            BindingOperations.SetBinding(line, Line.X2Property, endXbinding);

            Binding endYbinding = new Binding("Left");
            endYbinding.Mode = BindingMode.OneWay;
            endYbinding.Source = to;
            BindingOperations.SetBinding(line, Line.Y2Property, endYbinding);


        }
    }
}
