using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace JamesInk.Forms.Local.ViewModels
{
    public partial class EditorWindowViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<object> _CanvasItems;

        public EditorWindowViewModel()
        {
            CanvasItems = new();
            CanvasItems.Add(new object());
        }

        public void Onloaded()
        {
        }

        [RelayCommand]
        private void Add()
        {
            CanvasItems.Add(new object());
        }

        [RelayCommand]
        private void Close(object data)
        {
            CanvasItems.Remove(data);
        }
    }
}
