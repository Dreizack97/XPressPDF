using System;
using AppUI.Core.Services;
using AppUI.Layout;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Media;

namespace AppUI.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        NavbarControl.ToggleSidebarRequested += (_, _) => SidebarControl.ToggleCollapse();
        Loaded += (_, _) => ApplyWindowState(WindowState);
    }

    public void AttachServices(IStorageService storageService, IThemeService themeService, INavigationService navigationService, IToastService toastService)
    {
        storageService.RegisterTopLevel(this);
        NavbarControl.AttachServices(themeService);
        SidebarControl.AttachNavigationService(navigationService);
        ToastHostControl.AttachToastService(toastService);
    }

    public void ToggleSidebar() => SidebarControl.ToggleCollapse();

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);

        if (change.Property == WindowStateProperty && change.NewValue is WindowState state)
        {
            ApplyWindowState(state);
        }
    }

    private void ApplyWindowState(WindowState state)
    {
        if (WindowFrameBorder == null || ResizeGripsHost == null)
            return;

        var isMaximized = state == WindowState.Maximized || state == WindowState.FullScreen;

        if (isMaximized)
        {
            WindowFrameBorder.Margin = new Thickness(0);
            WindowFrameBorder.CornerRadius = new CornerRadius(0);
            WindowFrameBorder.BorderThickness = new Thickness(0);
            WindowFrameBorder.BoxShadow = default;
            ResizeGripsHost.IsVisible = false;
        }
        else
        {
            WindowFrameBorder.Margin = new Thickness(8);

            if (Application.Current?.TryGetResource("RadiusXl", Application.Current.ActualThemeVariant, out var radius) == true && radius is CornerRadius cr)
            {
                WindowFrameBorder.CornerRadius = cr;
            }
            else
            {
                WindowFrameBorder.CornerRadius = new CornerRadius(16);
            }

            WindowFrameBorder.BorderThickness = new Thickness(1);

            if (Application.Current?.TryGetResource("ElevationShadowLg", Application.Current.ActualThemeVariant, out var shadow) == true && shadow is BoxShadows bs)
            {
                WindowFrameBorder.BoxShadow = bs;
            }

            ResizeGripsHost.IsVisible = true;
        }
    }

    private void OnResizeGripPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        if (WindowState != WindowState.Normal)
            return;

        if (sender is Control control &&
            control.Tag is string tagStr &&
            Enum.TryParse<WindowEdge>(tagStr, out var edge))
        {
            BeginResizeDrag(edge, e);
        }
    }
}
