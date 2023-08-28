using ColorMixer.Contracts.Services;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using ColorMixer.Application.Controls;

namespace ColorMixer.Application.Views
{
    /// <summary>
    /// Interaction logic for ColorMixerView.xaml
    /// </summary>
    public partial class ColorMixerView : UserControl
    {
        private bool _isDown;
        private bool _isDragging;
        private UIElement _originalElement;
        private double _originalLeft;
        private double _originalTop;
        private CircleAdorner _overlayElement;
        private Point _startPoint;
        private TextBox? _lastTouchedTextBox;

        public ColorMixerView()
        {
            InitializeComponent();
            Binding binding = new Binding("[color_set]");
            binding.Mode = BindingMode.OneWay;
            binding.Source = LocalizationService.Instance;
            BindingOperations.SetBinding(this, TitleProperty, binding);
        }

        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register(
                nameof(Title),
                typeof(string),
                typeof(ColorMixerView),
                new PropertyMetadata("Defaul color set name"));


        public void OnLoaded(object sender, RoutedEventArgs e)
        {
            TextBox tb = new TextBox { Text = "Drag or select to change color" };
            Canvas.SetTop(tb, 100);
            Canvas.SetLeft(tb, 100);

            MixingCanvas.Children.Add(tb);

            MixingCanvas.PreviewMouseLeftButtonDown += MixingCanvas_PreviewMouseLeftButtonDown;
            MixingCanvas.PreviewMouseMove += MixingCanvas_PreviewMouseMove;
            MixingCanvas.PreviewMouseLeftButtonUp += MixingCanvas_PreviewMouseLeftButtonUp;
            //MixingCanvas.DragOver += MixingCanvas_DragOver;
            PreviewKeyDown += ColorMixer_PreviewKeyDown;
            MixingColorPicker.SelectedColorChanged += MixingColorPicker_SelectedColorChanged;
        }

        private void MixingColorPicker_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            if (e.NewValue != null && _lastTouchedTextBox != null)
            {
                _lastTouchedTextBox.Background = new SolidColorBrush(e.NewValue.Value);
                _lastTouchedTextBox.Text = e.NewValue.ToString();
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
            if (_isDown)
            {
                DragFinished(false);
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
            _isDragging = false;
            _isDown = false;
        }

        ///// <summary>
        ///// Forbides to drop outside the canvas.
        ///// </summary>
        //private void MixingCanvas_DragOver(object sender, DragEventArgs e)
        //{
        //    if (e.Source != MixingCanvas)
        //        e.Effects = DragDropEffects.None;
        //}

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
            _originalLeft = Canvas.GetLeft(_originalElement);
            _originalTop = Canvas.GetTop(_originalElement);
            _overlayElement = new CircleAdorner(_originalElement);
            AdornerLayer layer = AdornerLayer.GetAdornerLayer(_originalElement);
            layer.Add(_overlayElement);
        }

        private void DragMoved()
        {
            Point currentPosition = Mouse.GetPosition(MixingCanvas);

            _overlayElement.LeftOffset = currentPosition.X - _startPoint.X;
            _overlayElement.TopOffset = currentPosition.Y - _startPoint.Y;
        }

        private void MixingCanvas_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.Source == MixingCanvas)
            {
            }
            else
            {
                _isDown = true;
                _startPoint = e.GetPosition(MixingCanvas);
                _originalElement = e.Source as UIElement;
                MixingCanvas.CaptureMouse();
                e.Handled = true;

                if (e.Source is TextBox textBox)
                {
                    _lastTouchedTextBox = textBox;
                }
            }
        }

    }
}
