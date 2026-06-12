using Avalonia;

namespace AppUI.Avalonia
{
    internal static class Program
    {
        // No usar código que dependa de Avalonia antes de AppMain: todavía no hay framework inicializado.
        [STAThread]
        public static void Main(string[] args) => BuildAvaloniaApp()
            .StartWithClassicDesktopLifetime(args);

        public static AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .WithInterFont()
                .LogToTrace();
    }
}
