using AppUI.Core.Icons;
using AppUI.Core.Models;
using Avalonia;
using Avalonia.Controls.Primitives;

namespace AppUI.Shared.Controls.Containers;

public class ErpStatCard : TemplatedControl
{
    public static readonly StyledProperty<string> LabelProperty =
        AvaloniaProperty.Register<ErpStatCard, string>(nameof(Label), string.Empty);

    public static readonly StyledProperty<string> ValueProperty =
        AvaloniaProperty.Register<ErpStatCard, string>(nameof(Value), string.Empty);

    public static readonly StyledProperty<string?> DescriptionProperty =
        AvaloniaProperty.Register<ErpStatCard, string?>(nameof(Description));

    public static readonly StyledProperty<LucideIconKind> IconKindProperty =
        AvaloniaProperty.Register<ErpStatCard, LucideIconKind>(nameof(IconKind), LucideIconKind.Files);

    public static readonly StyledProperty<Tone> ToneProperty =
        AvaloniaProperty.Register<ErpStatCard, Tone>(nameof(Tone), Tone.Blue);

    public string Label
    {
        get => GetValue(LabelProperty);
        set => SetValue(LabelProperty, value);
    }

    public string Value
    {
        get => GetValue(ValueProperty);
        set => SetValue(ValueProperty, value);
    }

    public string? Description
    {
        get => GetValue(DescriptionProperty);
        set => SetValue(DescriptionProperty, value);
    }

    public LucideIconKind IconKind
    {
        get => GetValue(IconKindProperty);
        set => SetValue(IconKindProperty, value);
    }

    public Tone Tone
    {
        get => GetValue(ToneProperty);
        set => SetValue(ToneProperty, value);
    }
}
