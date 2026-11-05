using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Platform.Storage;

namespace AppUI.Core.Services;

public interface IStorageService
{
    void RegisterTopLevel(TopLevel topLevel);
    Task<IReadOnlyList<string>> PickXmlFilesAsync();
    Task<string?> PickFolderAsync();
    Task<string?> PickImageFileAsync();
}
