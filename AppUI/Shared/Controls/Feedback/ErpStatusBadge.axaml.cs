using AppUI.Core.Models;
using AppUI.Shared.Controls.Actions;
using Avalonia;
using Avalonia.Controls.Primitives;

namespace AppUI.Shared.Controls.Feedback;

public class ErpStatusBadge : TemplatedControl
{
    public static readonly StyledProperty<StatusTone> StatusProperty =
        AvaloniaProperty.Register<ErpStatusBadge, StatusTone>(nameof(Status), StatusTone.Neutral);

    public static readonly StyledProperty<string> TextProperty =
        AvaloniaProperty.Register<ErpStatusBadge, string>(nameof(Text), string.Empty);

    public static readonly StyledProperty<bool> ShowDotProperty =
        AvaloniaProperty.Register<ErpStatusBadge, bool>(nameof(ShowDot), true);

    public static readonly StyledProperty<ButtonSize> SizeProperty =
        AvaloniaProperty.Register<ErpStatusBadge, ButtonSize>(nameof(Size), ButtonSize.Md);

    public StatusTone Status
    {
        get => GetValue(StatusProperty);
        set => SetValue(StatusProperty, value);
    }

    public string Text
    {
        get => GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    public bool ShowDot
    {
        get => GetValue(ShowDotProperty);
        set => SetValue(ShowDotProperty, value);
    }

    public ButtonSize Size
    {
        get => GetValue(SizeProperty);
        set => SetValue(SizeProperty, value);
    }
}
