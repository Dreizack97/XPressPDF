using System.Threading.Tasks;
using AppUI.Core.Services;
using AppUI.Features.Conversion.ViewModels;
using AppUI.Features.Diagnostics.ViewModels;
using AppUI.Features.Settings.ViewModels;
using CommunityToolkit.Mvvm.ComponentModel;

namespace AppUI.Layout;

using global::Core.Abstractions;

public partial class MainWindowViewModel : ObservableObject
{
    private readonly INavigationService _navigationService;
    private readonly IPreferencesService _preferencesService;
    private readonly IThemeService _themeService;
    private readonly ILogService _logService;

    public ConversionWorkspaceViewModel ConversionVm { get; }
    public SettingsViewModel SettingsVm { get; }
    public DiagnosticsViewModel DiagnosticsVm { get; }

    [ObservableProperty]
    private object _currentView;

    public INavigationService NavigationService => _navigationService;
    public IThemeService ThemeService => _themeService;

    public MainWindowViewModel(
        INavigationService navigationService,
        IPreferencesService preferencesService,
        IThemeService themeService,
        ILogService logService,
        ConversionWorkspaceViewModel conversionVm,
        SettingsViewModel settingsVm,
        DiagnosticsViewModel diagnosticsVm)
    {
        _navigationService = navigationService;
        _preferencesService = preferencesService;
        _themeService = themeService;
        _logService = logService;

        ConversionVm = conversionVm;
        SettingsVm = settingsVm;
        DiagnosticsVm = diagnosticsVm;

        _currentView = ConversionVm;

        _navigationService.Navigated += OnNavigated;
    }

    public async Task InitializeAsync()
    {
        var prefs = await _preferencesService.LoadAsync();
        _themeService.SetTheme(prefs.SelectedTheme);
        await _logService.LogAsync("Sesión de usuario iniciada en interfaz gráfica.", "INFO").ConfigureAwait(false);
    }

    private void OnNavigated(NavigationTarget target)
    {
        CurrentView = target switch
        {
            NavigationTarget.Conversion => ConversionVm,
            NavigationTarget.Settings => SettingsVm,
            NavigationTarget.Diagnostics => DiagnosticsVm,
            _ => ConversionVm
        };

        if (target == NavigationTarget.Diagnostics)
        {
            DiagnosticsVm.LoadLogs();
        }
        else if (target == NavigationTarget.Settings)
        {
            SettingsVm.ReloadFromPreferences();
        }
    }
}
