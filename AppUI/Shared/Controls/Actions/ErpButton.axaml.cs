using System;
using AppUI.Core.Icons;
using Avalonia;
using Avalonia.Controls;

namespace AppUI.Shared.Controls.Actions;

public class ErpButton : Button
{
    public static readonly StyledProperty<ButtonVariant> VariantProperty =
        AvaloniaProperty.Register<ErpButton, ButtonVariant>(nameof(Variant), ButtonVariant.Primary);

    public static readonly StyledProperty<ButtonSize> SizeProperty =
        AvaloniaProperty.Register<ErpButton, ButtonSize>(nameof(Size), ButtonSize.Md);

    public static readonly StyledProperty<LucideIconKind?> LeadingIconProperty =
        AvaloniaProperty.Register<ErpButton, LucideIconKind?>(nameof(LeadingIcon));

    public static readonly StyledProperty<LucideIconKind?> TrailingIconProperty =
        AvaloniaProperty.Register<ErpButton, LucideIconKind?>(nameof(TrailingIcon));

    public static readonly StyledProperty<bool> IsLoadingProperty =
        AvaloniaProperty.Register<ErpButton, bool>(nameof(IsLoading));

    public static readonly StyledProperty<bool> IsIconOnlyProperty =
        AvaloniaProperty.Register<ErpButton, bool>(nameof(IsIconOnly));

    protected override Type StyleKeyOverride => typeof(ErpButton);

    public ButtonVariant Variant
    {
        get => GetValue(VariantProperty);
        set => SetValue(VariantProperty, value);
    }

    public ButtonSize Size
    {
        get => GetValue(SizeProperty);
        set => SetValue(SizeProperty, value);
    }

    public LucideIconKind? LeadingIcon
    {
        get => GetValue(LeadingIconProperty);
        set => SetValue(LeadingIconProperty, value);
    }

    public LucideIconKind? TrailingIcon
    {
        get => GetValue(TrailingIconProperty);
        set => SetValue(TrailingIconProperty, value);
    }

    public bool IsLoading
    {
        get => GetValue(IsLoadingProperty);
        set => SetValue(IsLoadingProperty, value);
    }

    public bool IsIconOnly
    {
        get => GetValue(IsIconOnlyProperty);
        set => SetValue(IsIconOnlyProperty, value);
    }
}
