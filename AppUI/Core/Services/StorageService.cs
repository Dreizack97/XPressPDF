using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Platform.Storage;

namespace AppUI.Core.Services;

public sealed class StorageService : IStorageService
{
    private TopLevel? _topLevel;

    public void RegisterTopLevel(TopLevel topLevel)
    {
        _topLevel = topLevel;
    }

    private IStorageProvider? StorageProvider => _topLevel?.StorageProvider;

    public async Task<IReadOnlyList<string>> PickXmlFilesAsync()
    {
        var provider = StorageProvider;
        if (provider == null) return Array.Empty<string>();

        var options = new FilePickerOpenOptions
        {
            Title = "Seleccionar comprobantes CFDI 4.0 (.xml)",
            AllowMultiple = true,
            FileTypeFilter = new[]
            {
                new FilePickerFileType("Archivos XML (*.xml)")
                {
                    Patterns = new[] { "*.xml" },
                    MimeTypes = new[] { "application/xml", "text/xml" }
                }
            }
        };

        var files = await provider.OpenFilePickerAsync(options);
        return files.Select(f => f.Path.LocalPath).ToList();
    }

    public async Task<string?> PickFolderAsync()
    {
        var provider = StorageProvider;
        if (provider == null) return null;

        var options = new FolderPickerOpenOptions
        {
            Title = "Seleccionar carpeta de destino para archivos PDF",
            AllowMultiple = false
        };

        var folders = await provider.OpenFolderPickerAsync(options);
        return folders.FirstOrDefault()?.Path.LocalPath;
    }

    public async Task<string?> PickImageFileAsync()
    {
        var provider = StorageProvider;
        if (provider == null) return null;

        var options = new FilePickerOpenOptions
        {
            Title = "Seleccionar logotipo corporativo",
            AllowMultiple = false,
            FileTypeFilter = new[]
            {
                new FilePickerFileType("Imágenes (*.png; *.jpg; *.jpeg; *.webp)")
                {
                    Patterns = new[] { "*.png", "*.jpg", "*.jpeg", "*.webp" },
                    MimeTypes = new[] { "image/png", "image/jpeg", "image/webp" }
                }
            }
        };

        var files = await provider.OpenFilePickerAsync(options);
        return files.FirstOrDefault()?.Path.LocalPath;
    }
}
