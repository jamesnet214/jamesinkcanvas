using JamesInk.Forms.Local.ViewModels;
using JamesInk.Support.Local.Mvvms;
using System.Windows;

namespace JamesInk.Forms.UI.Views
{
    public class EditorWindow : Window
    {
        static EditorWindow()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(EditorWindow), new FrameworkPropertyMetadata(typeof(EditorWindow)));
        }

        public EditorWindow()
        {
            DataContext = new EditorWindowViewModel();

            Loaded += EditorWindow_Loaded;
        }

        private void EditorWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is IViewloadable view)
            {
                view.Onloaded();
            }
        }
    }
}
