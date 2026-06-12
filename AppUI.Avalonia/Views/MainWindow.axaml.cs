using AppUI.Avalonia.ViewModels;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Platform.Storage;

namespace AppUI.Avalonia.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            AddHandler(DragDrop.DragOverEvent, OnDragOver);
            AddHandler(DragDrop.DropEvent, OnDrop);
        }

        private void OnDragOver(object? sender, DragEventArgs e)
        {
            e.DragEffects = e.Data.Contains(DataFormats.Files) ? DragDropEffects.Copy : DragDropEffects.None;
        }

        private void OnDrop(object? sender, DragEventArgs e)
        {
            if (DataContext is not MainViewModel viewModel)
                return;

            IEnumerable<string> paths = e.Data.GetFiles()
                ?.Select(item => item.TryGetLocalPath())
                .Where(path => !string.IsNullOrEmpty(path))
                .Select(path => path!)
                ?? [];

            viewModel.AddFiles(paths);
        }
    }
}
