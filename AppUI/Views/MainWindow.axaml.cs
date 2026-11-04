using AppUI.Core.Services;
using AppUI.Layout;
using Avalonia.Controls;

namespace AppUI.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        NavbarControl.ToggleSidebarRequested += (_, _) => SidebarControl.ToggleCollapse();
    }

    public void AttachServices(IStorageService storageService, IThemeService themeService, INavigationService navigationService, IToastService toastService)
    {
        storageService.RegisterTopLevel(this);
        NavbarControl.AttachServices(themeService);
        SidebarControl.AttachNavigationService(navigationService);
        ToastHostControl.AttachToastService(toastService);
    }

    public void ToggleSidebar() => SidebarControl.ToggleCollapse();
}
