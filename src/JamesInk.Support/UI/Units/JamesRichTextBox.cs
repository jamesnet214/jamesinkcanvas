using System.Collections;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace JamesInk.Support.UI.Units
{
    public class JamesRichTextBox : RichTextBox
    {
        static JamesRichTextBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(JamesRichTextBox), new FrameworkPropertyMetadata(typeof(JamesRichTextBox)));
        }

        public JamesRichTextBox()
        {
            Document = new FlowDocument();
        }

        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register("ItemsSource", typeof(IEnumerable), typeof(JamesRichTextBox), new FrameworkPropertyMetadata(null, OnItemsSourceChanged));

        public IEnumerable ItemsSource
        {
            get => (IEnumerable)GetValue(ItemsSourceProperty);
            set => SetValue(ItemsSourceProperty, value);
        }

        protected virtual Control GetContainerForItemOverride()
        {
            return new();
        }

        private static void OnItemsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is JamesRichTextBox richTextBox)
            {
                if (e.OldValue is INotifyCollectionChanged oldCollection)
                {
                    oldCollection.CollectionChanged -= richTextBox.OnCollectionChanged;
                }

                if (e.NewValue is INotifyCollectionChanged newCollection)
                {
                    newCollection.CollectionChanged += richTextBox.OnCollectionChanged;
                }

                richTextBox.UpdateFlowDocument();
            }
        }

        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (var newItem in e.NewItems)
                    {
                        AddToFlowDocument(newItem);
                    }
                    break;
                case NotifyCollectionChangedAction.Remove:
                    foreach (var oldItem in e.OldItems)
                    {
                        RemoveFromFlowDocument(oldItem);
                    }
                    break;
                case NotifyCollectionChangedAction.Reset:
                    UpdateFlowDocument();
                    break;
            }

            ScrollToEnd();
        }

        private void AddToFlowDocument(object item)
        {
            Control control = GetContainerForItemOverride();
            AttachFocusHandlers(control);
            control.DataContext = item;

            BlockUIContainer blockUI = new()
            {
                Margin = new Thickness(0),
                TextAlignment = TextAlignment.Center,
                Child = control
            };

            Document.Blocks.Add(blockUI);
        }

        private void RemoveFromFlowDocument(object item)
        {
            foreach (var block in Document.Blocks)
            {
                if (block is BlockUIContainer container && container.Child is Control control && control.DataContext == item)
                {
                    DetachFocusHandlers(control);
                    Document.Blocks.Remove(block);
                    break;
                }
            }
        }

        private void UpdateFlowDocument()
        {
            var existingItems = Document.Blocks
                .OfType<BlockUIContainer>()
                .Where(b => b.Child is Control)
                .Select(b => ((Control)b.Child).DataContext)
                .ToList();

            foreach (var existingItem in existingItems)
            {
                if (!ItemsSource.Cast<object>().Contains(existingItem))
                {
                    RemoveFromFlowDocument(existingItem);
                }
            }

            foreach (var item in ItemsSource)
            {
                if (!existingItems.Contains(item))
                {
                    AddToFlowDocument(item);
                }
            }

            ScrollToEnd();
        }

        private void AttachFocusHandlers(Control control)
        {
            control.GotFocus += Control_GotFocus;
            control.LostFocus += Control_LostFocus;
        }

        private void DetachFocusHandlers(Control control)
        {
            control.GotFocus -= Control_GotFocus;
            control.LostFocus -= Control_LostFocus;
        }

        private void Control_GotFocus(object sender, RoutedEventArgs e)
        {
            IsUndoEnabled = false;
        }

        private void Control_LostFocus(object sender, RoutedEventArgs e)
        {
            IsUndoEnabled = true;
        }
    }
}