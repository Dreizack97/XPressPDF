using AppUI.Core.Icons;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;

namespace AppUI.Shared.Controls.Containers;

public class ErpSectionCard : ContentControl
{
    public static readonly StyledProperty<string?> TitleProperty =
        AvaloniaProperty.Register<ErpSectionCard, string?>(nameof(Title));

    public static readonly StyledProperty<string?> SubtitleProperty =
        AvaloniaProperty.Register<ErpSectionCard, string?>(nameof(Subtitle));

    public static readonly StyledProperty<LucideIconKind?> IconKindProperty =
        AvaloniaProperty.Register<ErpSectionCard, LucideIconKind?>(nameof(IconKind));

    public static readonly StyledProperty<object?> HeaderActionsProperty =
        AvaloniaProperty.Register<ErpSectionCard, object?>(nameof(HeaderActions));

    public static readonly StyledProperty<bool> IsFlushProperty =
        AvaloniaProperty.Register<ErpSectionCard, bool>(nameof(IsFlush));

    public string? Title
    {
        get => GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    public string? Subtitle
    {
        get => GetValue(SubtitleProperty);
        set => SetValue(SubtitleProperty, value);
    }

    public LucideIconKind? IconKind
    {
        get => GetValue(IconKindProperty);
        set => SetValue(IconKindProperty, value);
    }

    public object? HeaderActions
    {
        get => GetValue(HeaderActionsProperty);
        set => SetValue(HeaderActionsProperty, value);
    }

    public bool IsFlush
    {
        get => GetValue(IsFlushProperty);
        set => SetValue(IsFlushProperty, value);
    }
}
