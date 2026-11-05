using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace AppUI.Shared.Converters;

public sealed class FileSizeFormatterConverter : IValueConverter
{
    private static readonly string[] Units = { "B", "KB", "MB", "GB" };

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not long bytes && value is not int && value is not double)
            return "0 B";

        double size = System.Convert.ToDouble(value);
        int unitIndex = 0;

        while (size >= 1024 && unitIndex < Units.Length - 1)
        {
            size /= 1024;
            unitIndex++;
        }

        return $"{size:0.##} {Units[unitIndex]}";
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) =>
        throw new NotSupportedException();
}
