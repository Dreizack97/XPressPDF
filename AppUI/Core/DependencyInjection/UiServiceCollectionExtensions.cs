using AppUI.Core.Services;
using AppUI.Features.Conversion.ViewModels;
using AppUI.Features.Diagnostics.ViewModels;
using AppUI.Features.Settings.ViewModels;
using AppUI.Layout;
using IoC;
using Microsoft.Extensions.DependencyInjection;

namespace AppUI.Core.DependencyInjection;

public static class UiServiceCollectionExtensions
{
    public static IServiceCollection AddAppUiServices(this IServiceCollection services)
    {
        // 1. Registrar motor Core de CFDI e IoC
        services.AddXPressPdfCore();

        // 2. Registrar servicios de infraestructura de UI (Singletons)
        services.AddSingleton<IThemeService, ThemeService>();
        services.AddSingleton<IPreferencesService, PreferencesService>();
        services.AddSingleton<IStorageService, StorageService>();
        services.AddSingleton<IProcessLauncherService, ProcessLauncherService>();
        services.AddSingleton<IToastService, ToastService>();
        services.AddSingleton<INavigationService, NavigationService>();

        // 3. Registrar ViewModels
        services.AddSingleton<ConversionWorkspaceViewModel>();
        services.AddSingleton<SettingsViewModel>();
        services.AddSingleton<DiagnosticsViewModel>();
        services.AddSingleton<MainWindowViewModel>();

        return services;
    }
}
