using AppUI.Avalonia.Models;
using AppUI.Avalonia.Services;
using BLL.Interfaces;
using BLL.Utilities;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Text;

namespace AppUI.Avalonia.ViewModels
{
    public partial class MainViewModel : ViewModelBase
    {
        private const int MaxConcurrentConversions = 4;

        private readonly IXmlReaderService _xmlReaderService;
        private readonly IFtpService _ftpService;
        private readonly IDialogService _dialogService;

        public ObservableCollection<XmlFileViewModel> Files { get; } = new();

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(AddFilesCommand))]
        [NotifyCanExecuteChangedFor(nameof(ConvertCommand))]
        [NotifyCanExecuteChangedFor(nameof(UploadCommand))]
        [NotifyCanExecuteChangedFor(nameof(ClearCommand))]
        private bool _isBusy;

        [ObservableProperty]
        private int _convertedCount;

        [ObservableProperty]
        private int _totalCount;

        public MainViewModel(IXmlReaderService xmlReaderService, IFtpService ftpService, IDialogService dialogService)
        {
            _xmlReaderService = xmlReaderService;
            _ftpService = ftpService;
            _dialogService = dialogService;

            Files.CollectionChanged += (_, _) =>
            {
                ConvertCommand.NotifyCanExecuteChanged();
                UploadCommand.NotifyCanExecuteChanged();
                ClearCommand.NotifyCanExecuteChanged();
            };
        }

        private bool CanInteract() => !IsBusy;

        private bool HasFiles() => !IsBusy && Files.Count > 0;

        [RelayCommand(CanExecute = nameof(CanInteract))]
        private async Task AddFilesAsync()
        {
            IReadOnlyList<string> paths = await _dialogService.PickXmlFilesAsync();
            AddFiles(paths);
        }

        /// <summary>Agrega archivos a la lista (selector o arrastrar y soltar), ignorando duplicados y no-XML.</summary>
        public void AddFiles(IEnumerable<string> paths)
        {
            foreach (string path in paths)
            {
                if (!string.Equals(Path.GetExtension(path), ".xml", StringComparison.OrdinalIgnoreCase))
                    continue;

                if (!File.Exists(path) || Files.Any(f => f.Path == path))
                    continue;

                Files.Add(new XmlFileViewModel(path));
            }
        }

        [RelayCommand(CanExecute = nameof(HasFiles))]
        private async Task ConvertAsync()
        {
            IsBusy = true;
            ConvertedCount = 0;
            TotalCount = Files.Count;

            try
            {
                using SemaphoreSlim throttle = new SemaphoreSlim(MaxConcurrentConversions);

                IEnumerable<Task> conversions = Files.Select(async file =>
                {
                    await throttle.WaitAsync();

                    try
                    {
                        file.Status = FileStatus.Converting;
                        await Task.Run(() => _xmlReaderService.Read(file.Path));

                        file.Status = FileStatus.Done;
                        file.ErrorMessage = null;
                    }
                    catch (Exception ex)
                    {
                        file.ErrorMessage = ex.InnerException?.Message ?? ex.Message;
                        file.Status = FileStatus.Error;
                    }
                    finally
                    {
                        throttle.Release();
                        ConvertedCount++;
                    }
                });

                await Task.WhenAll(conversions);
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand(CanExecute = nameof(HasFiles))]
        private async Task UploadAsync()
        {
            IsBusy = true;

            try
            {
                string[] pdfFiles = Files
                    .Select(file => Path.GetDirectoryName(file.Path)!)
                    .Where(directory => !string.IsNullOrEmpty(directory))
                    .Distinct()
                    .SelectMany(directory => Directory.GetFiles(directory, "*.pdf"))
                    .Distinct()
                    .ToArray();

                if (pdfFiles.Length == 0)
                {
                    await _dialogService.ShowMessageAsync("Subir PDF", "No se encontraron archivos PDF para subir. Convierte primero los XML.");
                    return;
                }

                string uploadPath = Path.Combine(DateTime.Now.Year.ToString(), "S", "1");
                var results = await _ftpService.UploadAsync(pdfFiles, uploadPath);

                int successCount = results.Count(r => r.Success);
                int failedCount = results.Count - successCount;

                StringBuilder message = new StringBuilder();
                message.AppendLine($"Archivos subidos: {successCount}");
                message.AppendLine($"Archivos fallidos: {failedCount}");
                message.Append($"Detalles en el log: {AppPaths.LogDirectory}");

                await _dialogService.ShowMessageAsync("Resultado de la subida FTP", message.ToString());
            }
            catch (Exception ex)
            {
                await _dialogService.ShowMessageAsync("Error", ex.Message);
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand(CanExecute = nameof(HasFiles))]
        private void Clear()
        {
            Files.Clear();
            ConvertedCount = 0;
            TotalCount = 0;
        }

        [RelayCommand(CanExecute = nameof(CanInteract))]
        private async Task OpenSettingsAsync() => await _dialogService.ShowSettingsAsync();
    }
}
