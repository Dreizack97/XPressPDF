using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using AppUI.Core.Models;
using AppUI.Core.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AppUI.Features.Settings.ViewModels;

public partial class SettingsViewModel : ObservableObject
{
    private readonly IPreferencesService _preferencesService;
    private readonly IThemeService _themeService;
    private readonly IStorageService _storageService;
    private readonly IToastService _toastService;

    [ObservableProperty]
    private string _selectedTheme = "System";

    public IReadOnlyList<string> ThemeOptions { get; } = ["System", "Light", "Dark"];

    [ObservableProperty]
    private bool _isCustomOutputDirectory;

    [ObservableProperty]
    private string? _customOutputDirectory;

    [ObservableProperty]
    private string? _logoPath;

    [ObservableProperty]
    private bool _overwriteExisting = true;

    [ObservableProperty]
    private bool _preScanMetadata = true;

    [ObservableProperty]
    private bool _reducedMotion;

    public SettingsViewModel(
        IPreferencesService preferencesService,
        IThemeService themeService,
        IStorageService storageService,
        IToastService toastService)
    {
        _preferencesService = preferencesService;
        _themeService = themeService;
        _storageService = storageService;
        _toastService = toastService;

        LoadCurrentPreferences();
    }

    private void LoadCurrentPreferences()
    {
        var prefs = _preferencesService.Current;
        SelectedTheme = prefs.SelectedTheme;
        CustomOutputDirectory = prefs.DefaultOutputDirectory;
        IsCustomOutputDirectory = !string.IsNullOrWhiteSpace(prefs.DefaultOutputDirectory);
        LogoPath = prefs.LogoPath;
        OverwriteExisting = prefs.OverwriteExisting;
        PreScanMetadata = prefs.PreScanMetadata;
        ReducedMotion = prefs.ReducedMotion;
    }

    partial void OnSelectedThemeChanged(string value)
    {
        _themeService.SetTheme(value);
    }

    [RelayCommand]
    private async Task PickFolderAsync()
    {
        var folder = await _storageService.PickFolderAsync();
        if (!string.IsNullOrEmpty(folder))
        {
            CustomOutputDirectory = folder;
            IsCustomOutputDirectory = true;
        }
    }

    [RelayCommand]
    private async Task PickLogoAsync()
    {
        var imagePath = await _storageService.PickImageFileAsync();
        if (!string.IsNullOrEmpty(imagePath))
        {
            LogoPath = imagePath;
        }
    }

    [RelayCommand]
    private void ClearLogo()
    {
        LogoPath = null;
    }

    [RelayCommand]
    private async Task SaveAsync()
    {
        var prefs = new AppPreferences
        {
            SelectedTheme = SelectedTheme,
            DefaultOutputDirectory = IsCustomOutputDirectory ? CustomOutputDirectory : null,
            LogoPath = LogoPath,
            OverwriteExisting = OverwriteExisting,
            PreScanMetadata = PreScanMetadata,
            ReducedMotion = ReducedMotion
        };

        await _preferencesService.SaveAsync(prefs);
        _toastService.Success("Preferencias Guardadas", "La configuración ha sido persistida exitosamente.");
    }
}
