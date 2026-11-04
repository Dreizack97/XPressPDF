using AppUI.Core.Services;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace AppUI.Layout.Sidebar;

public partial class ErpSidebar : UserControl
{
    public static readonly StyledProperty<bool> IsCollapsedProperty =
        AvaloniaProperty.Register<ErpSidebar, bool>(nameof(IsCollapsed));

    public static readonly StyledProperty<NavigationTarget> CurrentTargetProperty =
        AvaloniaProperty.Register<ErpSidebar, NavigationTarget>(nameof(CurrentTarget), NavigationTarget.Conversion);

    private INavigationService? _navigationService;

    public ErpSidebar()
    {
        InitializeComponent();
    }

    public bool IsCollapsed
    {
        get => GetValue(IsCollapsedProperty);
        set => SetValue(IsCollapsedProperty, value);
    }

    public NavigationTarget CurrentTarget
    {
        get => GetValue(CurrentTargetProperty);
        set => SetValue(CurrentTargetProperty, value);
    }

    public void AttachNavigationService(INavigationService navigationService)
    {
        _navigationService = navigationService;
        _navigationService.Navigated += target => CurrentTarget = target;
        CurrentTarget = _navigationService.CurrentTarget;
    }

    private void OnNavConversion(object? sender, RoutedEventArgs e) =>
        _navigationService?.NavigateTo(NavigationTarget.Conversion);

    private void OnNavSettings(object? sender, RoutedEventArgs e) =>
        _navigationService?.NavigateTo(NavigationTarget.Settings);

    private void OnNavDiagnostics(object? sender, RoutedEventArgs e) =>
        _navigationService?.NavigateTo(NavigationTarget.Diagnostics);

    private void OnToggleCollapse(object? sender, RoutedEventArgs e)
    {
        IsCollapsed = !IsCollapsed;
    }
}
