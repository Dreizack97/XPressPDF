using AppUI.Avalonia.Services;
using AppUI.Avalonia.ViewModels;
using AppUI.Avalonia.Views;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using BLL.Implementation;
using BLL.Interfaces;
using BLL.Utilities;
using Microsoft.Extensions.DependencyInjection;

namespace AppUI.Avalonia
{
    public partial class App : Application
    {
        public static IServiceProvider Services { get; private set; } = null!;

        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            Services = ConfigureServices();

            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = new MainWindow
                {
                    DataContext = Services.GetRequiredService<MainViewModel>()
                };
            }

            base.OnFrameworkInitializationCompleted();
        }

        private static ServiceProvider ConfigureServices()
        {
            ServiceCollection services = new ServiceCollection();

            // BLL
            services.AddSingleton<IConfigManager, ConfigManager>();
            services.AddSingleton<ILogManager, LogManager>();
            services.AddTransient<IXmlDeserializer, XmlDeserializer>();
            services.AddTransient<IComplementService, ComplementService>();
            services.AddTransient<IXmlReaderService, XmlReaderService>();
            services.AddTransient<IFtpService, FtpService>();
            services.AddTransient<IEmailService, EmailService>();

            // UI
            services.AddSingleton<IDialogService, DialogService>();
            services.AddTransient<MainViewModel>();
            services.AddTransient<SettingsViewModel>();
            services.AddTransient<FtpSettingsViewModel>();
            services.AddTransient<EmailSettingsViewModel>();

            return services.BuildServiceProvider();
        }
    }
}
