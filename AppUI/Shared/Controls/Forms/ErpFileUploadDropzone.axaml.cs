using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AppUI.Core.Icons;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;

namespace AppUI.Shared.Controls.Forms;

public partial class ErpFileUploadDropzone : UserControl
{
    public static readonly StyledProperty<string> TitleProperty =
        AvaloniaProperty.Register<ErpFileUploadDropzone, string>(nameof(Title), "Arrastra y suelta tus archivos CFDI 4.0 (.xml) aquí");

    public static readonly StyledProperty<string> DescriptionProperty =
        AvaloniaProperty.Register<ErpFileUploadDropzone, string>(nameof(Description), "o haz clic en los botones para seleccionar archivos o carpetas completas");

    public static readonly StyledProperty<LucideIconKind> IconKindProperty =
        AvaloniaProperty.Register<ErpFileUploadDropzone, LucideIconKind>(nameof(IconKind), LucideIconKind.Upload);

    public static readonly StyledProperty<bool> IsDragOverProperty =
        AvaloniaProperty.Register<ErpFileUploadDropzone, bool>(nameof(IsDragOver));

    public static readonly StyledProperty<System.Windows.Input.ICommand?> BrowseFilesCommandProperty =
        AvaloniaProperty.Register<ErpFileUploadDropzone, System.Windows.Input.ICommand?>(nameof(BrowseFilesCommand));

    public static readonly StyledProperty<System.Windows.Input.ICommand?> BrowseFolderCommandProperty =
        AvaloniaProperty.Register<ErpFileUploadDropzone, System.Windows.Input.ICommand?>(nameof(BrowseFolderCommand));

    public event Action<IReadOnlyList<string>>? FilesDropped;
    public event EventHandler<RoutedEventArgs>? BrowseFilesClicked;
    public event EventHandler<RoutedEventArgs>? BrowseFolderClicked;

    public ErpFileUploadDropzone()
    {
        InitializeComponent();

        DragDrop.SetAllowDrop(this, true);
        AddHandler(DragDrop.DragOverEvent, OnDragOver);
        AddHandler(DragDrop.DragLeaveEvent, OnDragLeave);
        AddHandler(DragDrop.DropEvent, OnDrop);
    }

    public System.Windows.Input.ICommand? BrowseFilesCommand
    {
        get => GetValue(BrowseFilesCommandProperty);
        set => SetValue(BrowseFilesCommandProperty, value);
    }

    public System.Windows.Input.ICommand? BrowseFolderCommand
    {
        get => GetValue(BrowseFolderCommandProperty);
        set => SetValue(BrowseFolderCommandProperty, value);
    }

    public string Title
    {
        get => GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    public string Description
    {
        get => GetValue(DescriptionProperty);
        set => SetValue(DescriptionProperty, value);
    }

    public LucideIconKind IconKind
    {
        get => GetValue(IconKindProperty);
        set => SetValue(IconKindProperty, value);
    }

    public bool IsDragOver
    {
        get => GetValue(IsDragOverProperty);
        private set => SetValue(IsDragOverProperty, value);
    }

    private void OnDragOver(object? sender, DragEventArgs e)
    {
        if (e.DataTransfer.Contains(DataFormat.File))
        {
            e.DragEffects = DragDropEffects.Copy;
            IsDragOver = true;
        }
        else
        {
            e.DragEffects = DragDropEffects.None;
            IsDragOver = false;
        }
    }

    private void OnDragLeave(object? sender, DragEventArgs e)
    {
        IsDragOver = false;
    }

    private void OnDrop(object? sender, DragEventArgs e)
    {
        IsDragOver = false;

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
                // Escaneo recursivo de carpeta
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

        if (paths.Count > 0)
        {
            FilesDropped?.Invoke(paths);
        }
    }

    private void OnBrowseFilesClick(object? sender, RoutedEventArgs e)
    {
        BrowseFilesClicked?.Invoke(this, e);
        if (BrowseFilesCommand?.CanExecute(null) == true)
        {
            BrowseFilesCommand.Execute(null);
        }
    }

    private void OnBrowseFolderClick(object? sender, RoutedEventArgs e)
    {
        BrowseFolderClicked?.Invoke(this, e);
        if (BrowseFolderCommand?.CanExecute(null) == true)
        {
            BrowseFolderCommand.Execute(null);
        }
    }
}
