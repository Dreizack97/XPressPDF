using System;
using AppUI.Core.Services;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace AppUI.Layout.Navbar;

public partial class ErpNavbar : UserControl
{
    public event EventHandler<RoutedEventArgs>? ToggleSidebarRequested;

    public ErpNavbar()
    {
        InitializeComponent();
    }

    public void AttachServices(IThemeService themeService)
    {
        ThemeToggleControl.AttachThemeService(themeService);
    }

    private void OnToggleSidebarClick(object? sender, RoutedEventArgs e)
    {
        ToggleSidebarRequested?.Invoke(this, e);
    }
}
