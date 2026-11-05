using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace AppUI.Shared.Controls.Overlays;

public partial class ErpDrawer : UserControl
{
    public static readonly StyledProperty<bool> IsOpenProperty =
        AvaloniaProperty.Register<ErpDrawer, bool>(nameof(IsOpen), defaultBindingMode: Avalonia.Data.BindingMode.TwoWay);

    public static readonly StyledProperty<string> TitleProperty =
        AvaloniaProperty.Register<ErpDrawer, string>(nameof(Title), "Detalle");

    public static readonly StyledProperty<object?> DrawerContentProperty =
        AvaloniaProperty.Register<ErpDrawer, object?>(nameof(DrawerContent));

    public ErpDrawer()
    {
        InitializeComponent();
    }

    public bool IsOpen
    {
        get => GetValue(IsOpenProperty);
        set => SetValue(IsOpenProperty, value);
    }

    public string Title
    {
        get => GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    public object? DrawerContent
    {
        get => GetValue(DrawerContentProperty);
        set => SetValue(DrawerContentProperty, value);
    }

    private void OnCloseClick(object? sender, RoutedEventArgs e)
    {
        IsOpen = false;
    }

    private void OnScrimPointerPressed(object? sender, Avalonia.Input.PointerPressedEventArgs e)
    {
        IsOpen = false;
    }
}
