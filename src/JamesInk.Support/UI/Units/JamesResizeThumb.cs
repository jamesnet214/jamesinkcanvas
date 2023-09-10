using System.Windows;
using System.Windows.Controls.Primitives;

namespace JamesInk.Support.UI.Units
{
    public class JamesResizeThumb : Thumb
    {
        private JamesInkCanvas canvas;

        static JamesResizeThumb()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(JamesResizeThumb), new FrameworkPropertyMetadata(typeof(JamesResizeThumb)));
        }

        internal void InitDragDelta(JamesInkCanvas jamesInkCanvas)
        {
            canvas = jamesInkCanvas;
            DragDelta += JamesResizeThumb_DragDelta;
        }

        private void JamesResizeThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            if (canvas.Width + e.HorizontalChange > canvas.MinWidth)
            {
                canvas.Width += e.HorizontalChange;
            }

            if (canvas.Height + e.VerticalChange > canvas.MinHeight)
            {
                canvas.Height += e.VerticalChange;
            }
        }
    }
}
