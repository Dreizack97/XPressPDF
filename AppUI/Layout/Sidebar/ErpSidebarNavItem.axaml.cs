using AppUI.Core.Icons;
using Avalonia;
using Avalonia.Controls;

namespace AppUI.Layout.Sidebar;

public class ErpSidebarNavItem : Button
{
    public static readonly StyledProperty<LucideIconKind> IconKindProperty =
        AvaloniaProperty.Register<ErpSidebarNavItem, LucideIconKind>(nameof(IconKind), LucideIconKind.House);

    public static readonly StyledProperty<string> TitleProperty =
        AvaloniaProperty.Register<ErpSidebarNavItem, string>(nameof(Title), string.Empty);

    public static readonly StyledProperty<string?> BadgeTextProperty =
        AvaloniaProperty.Register<ErpSidebarNavItem, string?>(nameof(BadgeText));

    public static readonly StyledProperty<bool> IsSelectedProperty =
        AvaloniaProperty.Register<ErpSidebarNavItem, bool>(nameof(IsSelected));

    public static readonly StyledProperty<bool> IsCollapsedProperty =
        AvaloniaProperty.Register<ErpSidebarNavItem, bool>(nameof(IsCollapsed));

    public LucideIconKind IconKind
    {
        get => GetValue(IconKindProperty);
        set => SetValue(IconKindProperty, value);
    }

    public string Title
    {
        get => GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    public string? BadgeText
    {
        get => GetValue(BadgeTextProperty);
        set => SetValue(BadgeTextProperty, value);
    }

    public bool IsSelected
    {
        get => GetValue(IsSelectedProperty);
        set => SetValue(IsSelectedProperty, value);
    }

    public bool IsCollapsed
    {
        get => GetValue(IsCollapsedProperty);
        set => SetValue(IsCollapsedProperty, value);
    }
}
