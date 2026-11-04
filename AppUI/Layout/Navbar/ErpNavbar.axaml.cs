using AppUI.Core.Services;
using Avalonia.Controls;

namespace AppUI.Layout.Navbar;

public partial class ErpNavbar : UserControl
{
    public ErpNavbar()
    {
        InitializeComponent();
    }

    public void AttachServices(IThemeService themeService)
    {
        ThemeToggleControl.AttachThemeService(themeService);
    }
}
