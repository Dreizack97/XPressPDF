using AppUI.Core.Services;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace AppUI.Layout.Sidebar;

public partial class ErpSidebar : UserControl
{
    private const double ExpandedWidth = 240.0;
    private const double CollapsedWidth = 72.0;

    public static readonly StyledProperty<bool> IsCollapsedProperty =
        AvaloniaProperty.Register<ErpSidebar, bool>(nameof(IsCollapsed), false);

    public static readonly StyledProperty<double> CurrentWidthProperty =
        AvaloniaProperty.Register<ErpSidebar, double>(nameof(CurrentWidth), ExpandedWidth);

    public static readonly StyledProperty<NavigationTarget> CurrentTargetProperty =
        AvaloniaProperty.Register<ErpSidebar, NavigationTarget>(nameof(CurrentTarget), NavigationTarget.Conversion);

    private INavigationService? _navigationService;
    private IPreferencesService? _preferencesService;

    static ErpSidebar()
    {
        IsCollapsedProperty.Changed.AddClassHandler<ErpSidebar>((s, e) => s.OnIsCollapsedChanged());
    }

    public ErpSidebar()
    {
        InitializeComponent();
        CurrentWidth = IsCollapsed ? CollapsedWidth : ExpandedWidth;
    }

    public bool IsCollapsed
    {
        get => GetValue(IsCollapsedProperty);
        set => SetValue(IsCollapsedProperty, value);
    }

    public double CurrentWidth
    {
        get => GetValue(CurrentWidthProperty);
        private set => SetValue(CurrentWidthProperty, value);
    }

    public NavigationTarget CurrentTarget
    {
        get => GetValue(CurrentTargetProperty);
        set => SetValue(CurrentTargetProperty, value);
    }

    public void ToggleCollapse()
    {
        IsCollapsed = !IsCollapsed;
    }

    private void OnIsCollapsedChanged()
    {
        CurrentWidth = IsCollapsed ? CollapsedWidth : ExpandedWidth;
        if (_preferencesService != null && _preferencesService.Current.IsSidebarCollapsed != IsCollapsed)
        {
            _preferencesService.Current.IsSidebarCollapsed = IsCollapsed;
            _preferencesService.Save(_preferencesService.Current);
        }
    }

    public void AttachServices(INavigationService navigationService, IPreferencesService preferencesService)
    {
        _preferencesService = preferencesService;
        IsCollapsed = preferencesService.Current.IsSidebarCollapsed;
        CurrentWidth = IsCollapsed ? CollapsedWidth : ExpandedWidth;

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
        ToggleCollapse();
    }
}
