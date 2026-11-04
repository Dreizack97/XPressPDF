using Avalonia;
using Avalonia.Controls;

namespace AppUI.Shared.Controls.Display;

public partial class ErpCodeBlock : UserControl
{
    public static readonly StyledProperty<string?> TextProperty =
        AvaloniaProperty.Register<ErpCodeBlock, string?>(nameof(Text));

    public ErpCodeBlock()
    {
        InitializeComponent();
    }

    public string? Text
    {
        get => GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }
}
