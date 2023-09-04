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
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace ColorMixer.Application.Views
{
    /// <summary>
    /// Interaction logic for ColorMixerView.xaml
    /// </summary>
    public partial class ColorMixerView : UserControl, IDisposable
    {
        private bool _isDown;
        private bool _isDragging;
        private ColorNodeControl? _originalElement;
        // todo - move this and show logic into ColorNodeControl
        private CircleAdorner _overlayElement;
        private Point _startPoint;

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

            Binding colorNodesBinding = new Binding("ColorNodes");
            colorNodesBinding.Mode = BindingMode.OneWay;
            colorNodesBinding.Source = DataContext;
            BindingOperations.SetBinding(this, ColorNodesProperty, colorNodesBinding);
        }

        public void OnLoaded(object sender, RoutedEventArgs e)
        {
            MixingCanvas.PreviewMouseLeftButtonDown += MixingCanvas_PreviewMouseLeftButtonDown;
            MixingCanvas.PreviewMouseMove += MixingCanvas_PreviewMouseMove;
            MixingCanvas.PreviewMouseLeftButtonUp += MixingCanvas_PreviewMouseLeftButtonUp;
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

        public ObservableCollection<IColorNode> ColorNodes
        {
            get { return (ObservableCollection<IColorNode>)GetValue(ColorNodesProperty); }
            set { SetValue(ColorNodesProperty, value); }
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

        public static readonly DependencyProperty ColorNodesProperty =
            DependencyProperty.Register(
                nameof(ColorNodes), 
                typeof(ObservableCollection<IColorNode>), 
                typeof(ColorMixerView), 
                new PropertyMetadata(null, new PropertyChangedCallback(OnColorNodesPropertySet)));

        #endregion

        private void MixingCanvas_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.Source is ColorNodeControl originalElement)
            {
                _originalElement = originalElement;
                SelectedColorNode = (IColorNode)_originalElement.DataContext;
                _isDown = true;
                _startPoint = e.GetPosition(MixingCanvas);
                MixingCanvas.CaptureMouse();
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
            if (_isDown && _isDragging && e.Source == MixingCanvas)
            {
                ColorNodeControl? target = MixingCanvas.FindChildByTypeAndPoint<ColorNodeControl>(e.GetPosition(App.Current.MainWindow));
                if (target != null && target.DataContext is IColorNode targetContext)
                {
                    TargetColorNode = targetContext;
                    if (TargetColorNode != SelectedColorNode)
                    {
                        MixColorsCommand.Execute(null);
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
                    Canvas.SetLeft(_originalElement, Canvas.GetLeft(_originalElement) + _overlayElement.LeftOffset);
                    Canvas.SetTop(_originalElement, Canvas.GetTop(_originalElement) + _overlayElement.TopOffset);
                }
                _overlayElement = null;
            }
        }

        private void MixingCanvas_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (_isDown)
            {
                if ((_isDragging == false) &&
                    ((Math.Abs(e.GetPosition(MixingCanvas).X - _startPoint.X) >
                      SystemParameters.MinimumHorizontalDragDistance) ||
                     (Math.Abs(e.GetPosition(MixingCanvas).Y - _startPoint.Y) >
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
            Point currentPosition = Mouse.GetPosition(MixingCanvas);
            _overlayElement.LeftOffset = currentPosition.X - _startPoint.X;
            _overlayElement.TopOffset = currentPosition.Y - _startPoint.Y;
        }

        private void CreateRelationLine(IColorNode from, IColorNode to)
        {
            Line line = new Line() 
            { 
                Stroke = new SolidColorBrush(Colors.Gray),
                StrokeThickness = 1.35,
            };

            Panel.SetZIndex(line, 10);

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
            BindingOperations.SetBinding(line, Line.Y2Property, endXbinding);

            Binding endYbinding = new Binding("Left");
            endYbinding.Mode = BindingMode.OneWay;
            endYbinding.Source = to;
            BindingOperations.SetBinding(line, Line.X2Property, endYbinding);
            
            MixingCanvas.Children.Add(line);
        }

        private static void OnColorNodesPropertySet(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ColorMixerView self = (ColorMixerView)d;

            if (e.OldValue is ObservableCollection<IColorNode> old)
            {
                old.CollectionChanged -= self.ColorNodes_CollectionChanged;
            }
            if (e.NewValue is ObservableCollection<IColorNode> newValue)
            {
                newValue.CollectionChanged += self.ColorNodes_CollectionChanged;
            }
        }

        private void ColorNodes_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems!= null)
            {
                foreach (object? item in e.NewItems)
                {
                    if (item is IColorNode colorNode)
                    {
                        ColorNodeControl control = new ColorNodeControl();
                        Panel.SetZIndex(control, 100);
                        control.DataContext = colorNode;

                        Binding colorBinding = new Binding("Color");
                        colorBinding.Mode = BindingMode.OneWay;
                        colorBinding.Source = colorNode;
                        BindingOperations.SetBinding(control, ColorNodeControl.ColorProperty, colorBinding);

                        Binding canvasTopBinding = new Binding("Top");
                        canvasTopBinding.Mode = BindingMode.TwoWay;
                        canvasTopBinding.Source = colorNode;
                        BindingOperations.SetBinding(control, Canvas.TopProperty, canvasTopBinding);

                        Binding canvasLeftBinding = new Binding("Left");
                        canvasLeftBinding.Mode = BindingMode.TwoWay;
                        canvasLeftBinding.Source = colorNode;
                        BindingOperations.SetBinding(control, Canvas.LeftProperty, canvasLeftBinding);

                        MixingCanvas.Children.Add(control);

                        if (colorNode.LeftParent != null && colorNode.RightParent != null)
                        {
                            CreateRelationLine(colorNode.LeftParent, colorNode);
                            CreateRelationLine(colorNode.RightParent, colorNode);
                        }
                    }
                }
            }
        }

        public void Dispose()
        {
            if (ColorNodes != null)
            {
                ColorNodes.CollectionChanged -= ColorNodes_CollectionChanged;
            }
            MixingCanvas.Children.Clear();
        }
    }
}
