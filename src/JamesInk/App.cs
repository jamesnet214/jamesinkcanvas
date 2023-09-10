using JamesInk.Forms.UI.Views;
using System.Windows;

namespace JamesInk
{
    internal class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            EditorWindow win = new();
            win.ShowDialog();
        }
    }
}
