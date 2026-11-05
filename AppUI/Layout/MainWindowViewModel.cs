using System.Threading.Tasks;
using AppUI.Core.Services;
using AppUI.Features.Conversion.ViewModels;
using AppUI.Features.Diagnostics.ViewModels;
using AppUI.Features.Settings.ViewModels;
using CommunityToolkit.Mvvm.ComponentModel;

namespace AppUI.Layout;

public partial class MainWindowViewModel : ObservableObject
{
    private readonly INavigationService _navigationService;
    private readonly IPreferencesService _preferencesService;
    private readonly IThemeService _themeService;

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
        ConversionWorkspaceViewModel conversionVm,
        SettingsViewModel settingsVm,
        DiagnosticsViewModel diagnosticsVm)
    {
        _navigationService = navigationService;
        _preferencesService = preferencesService;
        _themeService = themeService;

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
    }
}
