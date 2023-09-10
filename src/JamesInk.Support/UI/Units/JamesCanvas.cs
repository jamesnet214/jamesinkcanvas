using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace JamesInk.Support.UI.Units
{
    public class JamesCanvas : ContentControl
    {
        static JamesCanvas()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(JamesCanvas), new FrameworkPropertyMetadata(typeof(JamesCanvas)));
        }

        public static readonly DependencyProperty CanvasWidthProperty = DependencyProperty.Register(nameof(CanvasWidth), typeof(double), typeof(JamesCanvas), new PropertyMetadata(0.0));
        public static readonly DependencyProperty CanvasHeightProperty = DependencyProperty.Register(nameof(CanvasHeight), typeof(double), typeof(JamesCanvas), new PropertyMetadata(0.0));
        public static readonly DependencyProperty ViewboxOnProperty = DependencyProperty.Register(nameof(ViewboxOn), typeof(bool), typeof(JamesCanvas), new FrameworkPropertyMetadata(false, ViewboxOnPropertyChanged));

        public double CanvasWidth
        {
            get => (double)GetValue(CanvasWidthProperty);
            set => SetValue(CanvasWidthProperty, value);
        }


        public double CanvasHeight
        {
            get => (double)GetValue(CanvasHeightProperty);
            set => SetValue(CanvasHeightProperty, value);
        }

        public bool ViewboxOn
        {
            get => (bool)GetValue(ViewboxOnProperty);
            set => SetValue(ViewboxOnProperty, value);
        }

        private readonly JamesInkCanvas _jamesInkCanvas;
        public ContentControl _resizeContent;
        public ContentControl _viewboxContent;


        private static void ViewboxOnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            JamesCanvas james = (JamesCanvas)d;
            james._viewboxContent.Content = null;
            james._resizeContent.Content = null;

            switch (e.NewValue)
            {
                case true: james._viewboxContent.Content = james._jamesInkCanvas; break;
                default: james._resizeContent.Content = james._jamesInkCanvas; break;
            }
        }

        public JamesCanvas()
        {
            _jamesInkCanvas = new();
        }

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            base.OnPreviewKeyDown(e);
            _jamesInkCanvas.HandleUndoRedoKeyPress(e);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            JamesResizeThumb thumb = GetTemplateChild<JamesResizeThumb>("PART_Thumb");
            Button undoButton = GetTemplateChild<Button>("PART_UndoButton");
            Button redoButton = GetTemplateChild<Button>("PART_RedoButton");
            Button resetButton = GetTemplateChild<Button>("PART_ResetButton");
            _viewboxContent = GetTemplateChild<ContentControl>("PART_ViewboxContent");
            _resizeContent = GetTemplateChild<ContentControl>("PART_ResizeContent");

            _resizeContent.Content = _jamesInkCanvas;
            thumb.InitDragDelta(_jamesInkCanvas);
            undoButton.Click += (s, e) => _jamesInkCanvas.Undo();
            redoButton.Click += (s, e) => _jamesInkCanvas.Redo();
            resetButton.Click += (s, e) => _jamesInkCanvas.Reset();
        }

        private T GetTemplateChild<T>(string name) where T : DependencyObject
        {
            return (T)GetTemplateChild(name);
        }
    }
}
