using Avalonia;
using Avalonia.Controls;

namespace AppUI.Shared.Controls.Forms;

public class ErpField : ContentControl
{
    public static readonly StyledProperty<string?> LabelProperty =
        AvaloniaProperty.Register<ErpField, string?>(nameof(Label));

    public static readonly StyledProperty<string?> HintTextProperty =
        AvaloniaProperty.Register<ErpField, string?>(nameof(HintText));

    public static readonly StyledProperty<string?> ErrorTextProperty =
        AvaloniaProperty.Register<ErpField, string?>(nameof(ErrorText));

    public static readonly StyledProperty<bool> IsRequiredProperty =
        AvaloniaProperty.Register<ErpField, bool>(nameof(IsRequired));

    public string? Label
    {
        get => GetValue(LabelProperty);
        set => SetValue(LabelProperty, value);
    }

    public string? HintText
    {
        get => GetValue(HintTextProperty);
        set => SetValue(HintTextProperty, value);
    }

    public string? ErrorText
    {
        get => GetValue(ErrorTextProperty);
        set => SetValue(ErrorTextProperty, value);
    }

    public bool IsRequired
    {
        get => GetValue(IsRequiredProperty);
        set => SetValue(IsRequiredProperty, value);
    }
}
