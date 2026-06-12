using AppUI.Avalonia.ViewModels;
using AppUI.Avalonia.Views;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Layout;
using Avalonia.Platform.Storage;
using Microsoft.Extensions.DependencyInjection;

namespace AppUI.Avalonia.Services
{
    public class DialogService : IDialogService
    {
        private static Window MainWindow =>
            (global::Avalonia.Application.Current?.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime)?.MainWindow
                ?? throw new InvalidOperationException("Main window is not available.");

        public async Task<IReadOnlyList<string>> PickXmlFilesAsync()
        {
            IReadOnlyList<IStorageFile> files = await MainWindow.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
            {
                Title = "Seleccionar archivos CFDI",
                AllowMultiple = true,
                FileTypeFilter =
                [
                    new FilePickerFileType("Archivos XML") { Patterns = ["*.xml"] },
                    FilePickerFileTypes.All
                ]
            });

            return files
                .Select(file => file.TryGetLocalPath())
                .Where(path => !string.IsNullOrEmpty(path))
                .Select(path => path!)
                .ToList();
        }

        public async Task ShowMessageAsync(string title, string message)
        {
            Window dialog = new Window
            {
                Title = title,
                SizeToContent = SizeToContent.WidthAndHeight,
                MaxWidth = 480,
                CanResize = false,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                Content = BuildMessageContent(message, out Button okButton)
            };

            okButton.Click += (_, _) => dialog.Close();

            await dialog.ShowDialog(MainWindow);
        }

        public async Task ShowSettingsAsync()
        {
            SettingsWindow settings = new SettingsWindow
            {
                DataContext = App.Services.GetRequiredService<SettingsViewModel>()
            };

            await settings.ShowDialog(MainWindow);
        }

        private static Control BuildMessageContent(string message, out Button okButton)
        {
            okButton = new Button
            {
                Content = "Aceptar",
                HorizontalAlignment = HorizontalAlignment.Right,
                MinWidth = 90
            };

            return new StackPanel
            {
                Margin = new global::Avalonia.Thickness(20),
                Spacing = 15,
                Children =
                {
                    new TextBlock { Text = message, TextWrapping = global::Avalonia.Media.TextWrapping.Wrap },
                    okButton
                }
            };
        }
    }
}
