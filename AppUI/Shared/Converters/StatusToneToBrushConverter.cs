using System;
using System.Globalization;
using AppUI.Core.Models;
using Avalonia;
using Avalonia.Data.Converters;
using Avalonia.Media;

namespace AppUI.Shared.Converters;

public sealed class StatusToneToBrushConverter : IValueConverter
{
    public string Variant { get; set; } = "SurfaceSoft"; // SurfaceSoft, SurfaceSubtle, Border, Ink, InkStrong

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not StatusTone status) return null;
        var tone = status.ToTone();

        var key = $"Tone{tone}{Variant}Brush";
        if (Application.Current?.TryGetResource(key, Application.Current.ActualThemeVariant, out var res) == true && res is IBrush brush)
        {
            return brush;
        }

        return Brushes.Transparent;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) =>
        throw new NotSupportedException();
}
