using AppUI.Core.Models;
using Avalonia;
using Avalonia.Controls.Primitives;

namespace AppUI.Shared.Controls.Feedback;

public class ErpProgressBar : TemplatedControl
{
    public static readonly StyledProperty<double> ValueProperty =
        AvaloniaProperty.Register<ErpProgressBar, double>(nameof(Value));

    public static readonly StyledProperty<string?> LabelProperty =
        AvaloniaProperty.Register<ErpProgressBar, string?>(nameof(Label));

    public static readonly StyledProperty<Tone> ToneProperty =
        AvaloniaProperty.Register<ErpProgressBar, Tone>(nameof(Tone), Tone.Blue);

    public static readonly StyledProperty<bool> IsIndeterminateProperty =
        AvaloniaProperty.Register<ErpProgressBar, bool>(nameof(IsIndeterminate));

    public double Value
    {
        get => GetValue(ValueProperty);
        set => SetValue(ValueProperty, value);
    }

    public string? Label
    {
        get => GetValue(LabelProperty);
        set => SetValue(LabelProperty, value);
    }

    public Tone Tone
    {
        get => GetValue(ToneProperty);
        set => SetValue(ToneProperty, value);
    }

    public bool IsIndeterminate
    {
        get => GetValue(IsIndeterminateProperty);
        set => SetValue(IsIndeterminateProperty, value);
    }
}
