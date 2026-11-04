using AppUI.Core.Icons;
using AppUI.Core.Services;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Styling;

namespace AppUI.Shared.Controls.Actions;

public partial class ErpThemeToggle : UserControl
{
    public static readonly StyledProperty<LucideIconKind> IconKindProperty =
        AvaloniaProperty.Register<ErpThemeToggle, LucideIconKind>(nameof(IconKind), LucideIconKind.Moon);

    public static readonly StyledProperty<string> TooltipTextProperty =
        AvaloniaProperty.Register<ErpThemeToggle, string>(nameof(TooltipText), "Cambiar tema (Claro / Oscuro)");

    private IThemeService? _themeService;

    public ErpThemeToggle()
    {
        InitializeComponent();
        UpdateIcon();
        ActualThemeVariantChanged += (_, _) => UpdateIcon();
    }

    public LucideIconKind IconKind
    {
        get => GetValue(IconKindProperty);
        private set => SetValue(IconKindProperty, value);
    }

    public string TooltipText
    {
        get => GetValue(TooltipTextProperty);
        private set => SetValue(TooltipTextProperty, value);
    }

    public void AttachThemeService(IThemeService themeService)
    {
        _themeService = themeService;
        _themeService.ThemeChanged += _ => UpdateIcon();
        UpdateIcon();
    }

    private void UpdateIcon()
    {
        var isDark = ActualThemeVariant == ThemeVariant.Dark;
        IconKind = isDark ? LucideIconKind.Sun : LucideIconKind.Moon;
        TooltipText = isDark ? "Cambiar a modo claro" : "Cambiar a modo oscuro";
    }

    private void OnClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        _themeService?.ToggleTheme();
    }
}
