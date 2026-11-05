using System;
using AppUI.Core.Icons;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace AppUI.Layout.Navbar;

public partial class ErpWindowControls : UserControl
{
    public static readonly StyledProperty<LucideIconKind> MaximizeRestoreIconProperty =
        AvaloniaProperty.Register<ErpWindowControls, LucideIconKind>(nameof(MaximizeRestoreIcon), LucideIconKind.Square);

    public static readonly StyledProperty<string> MaximizeRestoreTooltipProperty =
        AvaloniaProperty.Register<ErpWindowControls, string>(nameof(MaximizeRestoreTooltip), "Maximizar");

    private Window? _parentWindow;

    public ErpWindowControls()
    {
        InitializeComponent();
    }

    public LucideIconKind MaximizeRestoreIcon
    {
        get => GetValue(MaximizeRestoreIconProperty);
        private set => SetValue(MaximizeRestoreIconProperty, value);
    }

    public string MaximizeRestoreTooltip
    {
        get => GetValue(MaximizeRestoreTooltipProperty);
        private set => SetValue(MaximizeRestoreTooltipProperty, value);
    }

    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);
        _parentWindow = TopLevel.GetTopLevel(this) as Window;

        if (_parentWindow != null)
        {
            _parentWindow.PropertyChanged += OnWindowPropertyChanged;
            UpdateState(_parentWindow.WindowState);
        }
    }

    protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
    {
        if (_parentWindow != null)
        {
            _parentWindow.PropertyChanged -= OnWindowPropertyChanged;
            _parentWindow = null;
        }

        base.OnDetachedFromVisualTree(e);
    }

    private void OnWindowPropertyChanged(object? sender, AvaloniaPropertyChangedEventArgs e)
    {
        if (e.Property == Window.WindowStateProperty && e.NewValue is WindowState state)
        {
            UpdateState(state);
        }
    }

    private void UpdateState(WindowState state)
    {
        var isMaximized = state == WindowState.Maximized || state == WindowState.FullScreen;
        MaximizeRestoreIcon = isMaximized ? LucideIconKind.Copy : LucideIconKind.Square;
        MaximizeRestoreTooltip = isMaximized ? "Restaurar tamaño" : "Maximizar";
    }

    private void OnMinimizeClick(object? sender, RoutedEventArgs e)
    {
        if (_parentWindow != null)
        {
            _parentWindow.WindowState = WindowState.Minimized;
        }
    }

    private void OnMaximizeRestoreClick(object? sender, RoutedEventArgs e)
    {
        if (_parentWindow != null)
        {
            _parentWindow.WindowState = _parentWindow.WindowState == WindowState.Maximized
                ? WindowState.Normal
                : WindowState.Maximized;
        }
    }

    private void OnCloseClick(object? sender, RoutedEventArgs e)
    {
        _parentWindow?.Close();
    }
}
