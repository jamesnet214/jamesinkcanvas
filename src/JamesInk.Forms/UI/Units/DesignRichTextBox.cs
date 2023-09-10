using JamesInk.Support.UI.Units;
using System.Windows;
using System.Windows.Controls;

namespace JamesInk.Forms.UI.Units
{
    public class DesignRichTextBox : JamesRichTextBox
    {
        static DesignRichTextBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DesignRichTextBox), new FrameworkPropertyMetadata(typeof(DesignRichTextBox)));
        }
        protected override Control GetContainerForItemOverride()
        {
            return new JamesCanvas();
        }
    }
}
