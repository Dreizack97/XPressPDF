using AppUI.Avalonia.Models;
using Avalonia.Data.Converters;
using Avalonia.Media;
using System.Globalization;

namespace AppUI.Avalonia.Converters
{
    public class FileStatusToBrushConverter : IValueConverter
    {
        private static readonly IBrush PendingBrush = new SolidColorBrush(Color.Parse("#8E8E93"));
        private static readonly IBrush ConvertingBrush = new SolidColorBrush(Color.Parse("#007AFF"));
        private static readonly IBrush DoneBrush = new SolidColorBrush(Color.Parse("#34C759"));
        private static readonly IBrush ErrorBrush = new SolidColorBrush(Color.Parse("#FF3B30"));

        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture) => value switch
        {
            FileStatus.Converting => ConvertingBrush,
            FileStatus.Done => DoneBrush,
            FileStatus.Error => ErrorBrush,
            _ => PendingBrush
        };

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) =>
            throw new NotSupportedException();
    }
}
