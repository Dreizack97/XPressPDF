using System;
using AppUI.Core.DependencyInjection;
using AppUI.Core.Services;
using AppUI.Layout;
using AppUI.Views;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;

namespace AppUI;

public partial class App : Application
{
    public IServiceProvider? Services { get; private set; }

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override async void OnFrameworkInitializationCompleted()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddAppUiServices();
        Services = serviceCollection.BuildServiceProvider();

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            var mainVm = Services.GetRequiredService<MainWindowViewModel>();
            await mainVm.InitializeAsync();

            var mainWindow = new MainWindow
            {
                DataContext = mainVm
            };

            var storageService = Services.GetRequiredService<IStorageService>();
            var themeService = Services.GetRequiredService<IThemeService>();
            var navService = Services.GetRequiredService<INavigationService>();
            var toastService = Services.GetRequiredService<IToastService>();

            mainWindow.AttachServices(storageService, themeService, navService, toastService);

            desktop.MainWindow = mainWindow;
        }

        base.OnFrameworkInitializationCompleted();
    }
}
