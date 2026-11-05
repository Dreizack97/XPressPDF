using Avalonia;
using Avalonia.Controls;

namespace AppUI.Layout.PageHeader;

public class ErpPageHeader : ContentControl
{
    public static readonly StyledProperty<string?> CategoryProperty =
        AvaloniaProperty.Register<ErpPageHeader, string?>(nameof(Category));

    public static readonly StyledProperty<string> TitleProperty =
        AvaloniaProperty.Register<ErpPageHeader, string>(nameof(Title), string.Empty);

    public static readonly StyledProperty<string?> SubtitleProperty =
        AvaloniaProperty.Register<ErpPageHeader, string?>(nameof(Subtitle));

    public static readonly StyledProperty<object?> PrimaryActionsProperty =
        AvaloniaProperty.Register<ErpPageHeader, object?>(nameof(PrimaryActions));

    public string? Category
    {
        get => GetValue(CategoryProperty);
        set => SetValue(CategoryProperty, value);
    }

    public string Title
    {
        get => GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    public string? Subtitle
    {
        get => GetValue(SubtitleProperty);
        set => SetValue(SubtitleProperty, value);
    }

    public object? PrimaryActions
    {
        get => GetValue(PrimaryActionsProperty);
        set => SetValue(PrimaryActionsProperty, value);
    }
}
