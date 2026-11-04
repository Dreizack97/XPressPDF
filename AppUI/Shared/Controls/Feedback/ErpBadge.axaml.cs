using AppUI.Core.Models;
using AppUI.Shared.Controls.Actions;
using Avalonia;
using Avalonia.Controls.Primitives;

namespace AppUI.Shared.Controls.Feedback;

public class ErpBadge : TemplatedControl
{
    public static readonly StyledProperty<Tone> ToneProperty =
        AvaloniaProperty.Register<ErpBadge, Tone>(nameof(Tone), Tone.Neutral);

    public static readonly StyledProperty<string> TextProperty =
        AvaloniaProperty.Register<ErpBadge, string>(nameof(Text), string.Empty);

    public static readonly StyledProperty<ButtonSize> SizeProperty =
        AvaloniaProperty.Register<ErpBadge, ButtonSize>(nameof(Size), ButtonSize.Md);

    public Tone Tone
    {
        get => GetValue(ToneProperty);
        set => SetValue(ToneProperty, value);
    }

    public string Text
    {
        get => GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    public ButtonSize Size
    {
        get => GetValue(SizeProperty);
        set => SetValue(SizeProperty, value);
    }
}
