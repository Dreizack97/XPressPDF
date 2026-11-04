using System;
using System.Collections.Generic;
using System.IO;
using AppUI.Features.Conversion.ViewModels;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;

namespace AppUI.Features.Conversion.Views;

public partial class ConversionWorkspaceView : UserControl
{
    public static readonly StyledProperty<bool> IsDragOverActiveProperty =
        AvaloniaProperty.Register<ConversionWorkspaceView, bool>(nameof(IsDragOverActive), false);

    public bool IsDragOverActive
    {
        get => GetValue(IsDragOverActiveProperty);
        set => SetValue(IsDragOverActiveProperty, value);
    }

    public ConversionWorkspaceView()
    {
        InitializeComponent();

        DragDrop.SetAllowDrop(this, true);
        AddHandler(DragDrop.DragOverEvent, OnDragOver);
        AddHandler(DragDrop.DragLeaveEvent, OnDragLeave);
        AddHandler(DragDrop.DropEvent, OnDrop);
    }

    private void OnDragOver(object? sender, DragEventArgs e)
    {
        if (e.DataTransfer.Contains(DataFormat.File))
        {
            e.DragEffects = DragDropEffects.Copy;
            IsDragOverActive = true;
        }
        else
        {
            e.DragEffects = DragDropEffects.None;
            IsDragOverActive = false;
        }
    }

    private void OnDragLeave(object? sender, DragEventArgs e)
    {
        IsDragOverActive = false;
    }

    private void OnDrop(object? sender, DragEventArgs e)
    {
        IsDragOverActive = false;

        var files = e.DataTransfer.TryGetFiles();
        if (files == null) return;

        var paths = new List<string>();
        foreach (var file in files)
        {
            var localPath = file.Path.LocalPath;
            if (File.Exists(localPath) && localPath.EndsWith(".xml", StringComparison.OrdinalIgnoreCase))
            {
                paths.Add(localPath);
            }
            else if (Directory.Exists(localPath))
            {
                try
                {
                    var xmls = Directory.GetFiles(localPath, "*.xml", SearchOption.AllDirectories);
                    paths.AddRange(xmls);
                }
                catch
                {
                    // Ignorar carpetas sin permisos de lectura
                }
            }
        }

        if (paths.Count > 0 && DataContext is ConversionWorkspaceViewModel vm)
        {
            vm.AddXmlFiles(paths);
        }
    }

    private void OnResetFiltersClick(object? sender, RoutedEventArgs e)
    {
        if (DataContext is ConversionWorkspaceViewModel vm)
        {
            vm.SearchQuery = string.Empty;
            vm.SelectedStatusFilter = "Todos";
            vm.SelectedTypeFilter = "Todos";
        }
    }
}
