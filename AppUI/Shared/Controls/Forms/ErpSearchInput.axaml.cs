using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace AppUI.Shared.Controls.Forms;

public partial class ErpSearchInput : UserControl
{
    public static readonly StyledProperty<string?> TextProperty =
        AvaloniaProperty.Register<ErpSearchInput, string?>(nameof(Text), defaultBindingMode: Avalonia.Data.BindingMode.TwoWay);

    public static readonly StyledProperty<string> PlaceholderTextProperty =
        AvaloniaProperty.Register<ErpSearchInput, string>(nameof(PlaceholderText), "Buscar...");

    public event EventHandler<string?>? SearchTextChanged;

    public ErpSearchInput()
    {
        InitializeComponent();
    }

    public string? Text
    {
        get => GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    public string PlaceholderText
    {
        get => GetValue(PlaceholderTextProperty);
        set => SetValue(PlaceholderTextProperty, value);
    }

    private void OnClearClick(object? sender, RoutedEventArgs e)
    {
        Text = string.Empty;
        SearchTextChanged?.Invoke(this, string.Empty);
    }
}
