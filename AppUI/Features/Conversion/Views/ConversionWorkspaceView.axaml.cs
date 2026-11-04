using System.Collections.Generic;
using AppUI.Features.Conversion.ViewModels;
using Avalonia.Controls;

namespace AppUI.Features.Conversion.Views;

public partial class ConversionWorkspaceView : UserControl
{
    public ConversionWorkspaceView()
    {
        InitializeComponent();
    }

    private void OnDropzoneFilesDropped(IReadOnlyList<string> paths)
    {
        if (DataContext is ConversionWorkspaceViewModel vm)
        {
            vm.AddXmlFiles(paths);
        }
    }
}
