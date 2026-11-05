using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
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
        var pos = e.GetPosition(this);
        if (pos.X < 0 || pos.Y < 0 || pos.X >= Bounds.Width || pos.Y >= Bounds.Height)
        {
            IsDragOverActive = false;
        }
    }

    private async void OnDrop(object? sender, DragEventArgs e)
    {
        IsDragOverActive = false;

        var files = e.DataTransfer.TryGetFiles();
        if (files == null) return;

        var rawPaths = new List<string>();
        foreach (var file in files)
        {
            rawPaths.Add(file.Path.LocalPath);
        }

        if (rawPaths.Count > 0 && DataContext is ConversionWorkspaceViewModel vm)
        {
            var resolvedPaths = await Task.Run(() =>
            {
                var result = new List<string>();
                foreach (var path in rawPaths)
                {
                    if (File.Exists(path) && path.EndsWith(".xml", StringComparison.OrdinalIgnoreCase))
                    {
                        result.Add(path);
                    }
                    else if (Directory.Exists(path))
                    {
                        try
                        {
                            var xmls = Directory.GetFiles(path, "*.xml", SearchOption.AllDirectories);
                            result.AddRange(xmls);
                        }
                        catch
                        {
                            // Ignorar directorios sin permisos
                        }
                    }
                }
                return result;
            });

            if (resolvedPaths.Count > 0)
            {
                vm.AddXmlFiles(resolvedPaths);
            }
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
