using AppUI.Core.Icons;
using Avalonia;
using Avalonia.Controls;

namespace AppUI.Shared.Controls.Feedback;

public class ErpEmptyState : ContentControl
{
    public static readonly StyledProperty<LucideIconKind> IconKindProperty =
        AvaloniaProperty.Register<ErpEmptyState, LucideIconKind>(nameof(IconKind), LucideIconKind.Files);

    public static readonly StyledProperty<string> TitleProperty =
        AvaloniaProperty.Register<ErpEmptyState, string>(nameof(Title), string.Empty);

    public static readonly StyledProperty<string> DescriptionProperty =
        AvaloniaProperty.Register<ErpEmptyState, string>(nameof(Description), string.Empty);

    public LucideIconKind IconKind
    {
        get => GetValue(IconKindProperty);
        set => SetValue(IconKindProperty, value);
    }

    public string Title
    {
        get => GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    public string Description
    {
        get => GetValue(DescriptionProperty);
        set => SetValue(DescriptionProperty, value);
    }
}
