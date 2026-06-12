using AppUI.Avalonia.Models;
using CommunityToolkit.Mvvm.ComponentModel;

namespace AppUI.Avalonia.ViewModels
{
    public partial class XmlFileViewModel : ViewModelBase
    {
        public string Name { get; }

        public string Path { get; }

        public string Size { get; }

        public DateTime ModificationDate { get; }

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(StatusText))]
        private FileStatus _status = FileStatus.Pending;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(StatusText))]
        private string? _errorMessage;

        public string StatusText => Status switch
        {
            FileStatus.Pending => "Pendiente",
            FileStatus.Converting => "Convirtiendo…",
            FileStatus.Done => "Completado",
            FileStatus.Error => ErrorMessage ?? "Error",
            _ => string.Empty
        };

        public XmlFileViewModel(string filePath)
        {
            FileInfo fileInfo = new FileInfo(filePath);

            Name = fileInfo.Name;
            Path = fileInfo.FullName;
            Size = $"{Math.Max(1, fileInfo.Length / 1024)} KB";
            ModificationDate = fileInfo.LastWriteTime;
        }
    }
}
