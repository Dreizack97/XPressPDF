using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;

namespace AppUI.Shared.Controls.Forms;

public partial class ErpPathPickerField : UserControl
{
    public static readonly StyledProperty<string?> SelectedPathProperty =
        AvaloniaProperty.Register<ErpPathPickerField, string?>(nameof(SelectedPath), defaultBindingMode: Avalonia.Data.BindingMode.TwoWay);

    public static readonly StyledProperty<string> PlaceholderTextProperty =
        AvaloniaProperty.Register<ErpPathPickerField, string>(nameof(PlaceholderText), "Seleccionar ruta...");

    public static readonly StyledProperty<string> ButtonTextProperty =
        AvaloniaProperty.Register<ErpPathPickerField, string>(nameof(ButtonText), "Examinar...");

    public static readonly StyledProperty<ICommand?> BrowseCommandProperty =
        AvaloniaProperty.Register<ErpPathPickerField, ICommand?>(nameof(BrowseCommand));

    public ErpPathPickerField()
    {
        InitializeComponent();
    }

    public string? SelectedPath
    {
        get => GetValue(SelectedPathProperty);
        set => SetValue(SelectedPathProperty, value);
    }

    public string PlaceholderText
    {
        get => GetValue(PlaceholderTextProperty);
        set => SetValue(PlaceholderTextProperty, value);
    }

    public string ButtonText
    {
        get => GetValue(ButtonTextProperty);
        set => SetValue(ButtonTextProperty, value);
    }

    public ICommand? BrowseCommand
    {
        get => GetValue(BrowseCommandProperty);
        set => SetValue(BrowseCommandProperty, value);
    }
}
