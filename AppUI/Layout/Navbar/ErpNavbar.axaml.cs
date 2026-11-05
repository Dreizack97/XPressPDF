using System;
using AppUI.Core.Services;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.VisualTree;

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

    private void OnNavbarPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        if (e.GetCurrentPoint(this).Properties.IsLeftButtonPressed)
        {
            if (e.Source is Avalonia.Visual visual)
            {
                // Si el clic proviene de un botón o control interactivo (o sus descendientes), no iniciar arrastre
                if (visual.FindAncestorOfType<Button>(includeSelf: true) != null ||
                    visual.FindAncestorOfType<ToggleButton>(includeSelf: true) != null)
                {
                    return;
                }
            }

            var window = TopLevel.GetTopLevel(this) as Window;
            if (window == null) return;

            if (e.ClickCount == 2)
            {
                // Doble clic: Alternar entre Maximizado y Normal
                window.WindowState = window.WindowState == WindowState.Maximized
                    ? WindowState.Normal
                    : WindowState.Maximized;
            }
            else if (e.ClickCount == 1)
            {
                // Clic sostenido: Arrastre nativo de ventana
                window.BeginMoveDrag(e);
            }
        }
    }
}
