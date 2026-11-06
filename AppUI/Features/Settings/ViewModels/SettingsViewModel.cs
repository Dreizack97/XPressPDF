using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using AppUI.Core.Models;
using AppUI.Core.Services;
using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AppUI.Features.Settings.ViewModels;

public partial class SettingsViewModel : ObservableObject
{
    private readonly IPreferencesService _preferencesService;
    private readonly IThemeService _themeService;
    private readonly IStorageService _storageService;
    private readonly IToastService _toastService;
    private bool _isReloading;

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
    private Bitmap? _logoPreview;

    public bool HasLogoPreview => LogoPreview != null;

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
        _preferencesService.PreferencesChanged += OnPreferencesChanged;
    }

    private void LoadCurrentPreferences()
    {
        var prefs = _preferencesService.Current;
        SelectedTheme = prefs.SelectedTheme;
        CustomOutputDirectory = prefs.DefaultOutputDirectory;
        IsCustomOutputDirectory = !string.IsNullOrWhiteSpace(prefs.DefaultOutputDirectory);
        LogoPath = prefs.LogoPath;
        UpdateLogoPreview(prefs.LogoPath);
        OverwriteExisting = prefs.OverwriteExisting;
        PreScanMetadata = prefs.PreScanMetadata;
        ReducedMotion = prefs.ReducedMotion;
    }

    public void ReloadFromPreferences()
    {
        _isReloading = true;
        try
        {
            LoadCurrentPreferences();
        }
        finally
        {
            _isReloading = false;
        }
    }

    private void OnPreferencesChanged(AppPreferences prefs)
    {
        if (_isReloading) return;
        if (SelectedTheme != prefs.SelectedTheme)
        {
            _isReloading = true;
            try
            {
                SelectedTheme = prefs.SelectedTheme;
            }
            finally
            {
                _isReloading = false;
            }
        }
    }

    partial void OnSelectedThemeChanged(string value)
    {
        _themeService.SetTheme(value);
        AutoSave();
    }

    partial void OnIsCustomOutputDirectoryChanged(bool value) => AutoSave();
    partial void OnCustomOutputDirectoryChanged(string? value) => AutoSave();

    partial void OnLogoPathChanged(string? value)
    {
        UpdateLogoPreview(value);
        AutoSave();
    }

    partial void OnOverwriteExistingChanged(bool value) => AutoSave();
    partial void OnPreScanMetadataChanged(bool value) => AutoSave();
    partial void OnReducedMotionChanged(bool value) => AutoSave();

    private void UpdateLogoPreview(string? path)
    {
        LogoPreview?.Dispose();
        LogoPreview = null;

        if (!string.IsNullOrWhiteSpace(path) && File.Exists(path))
        {
            try
            {
                using var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                LogoPreview = new Bitmap(fs);
            }
            catch
            {
                LogoPreview = null;
            }
        }

        OnPropertyChanged(nameof(HasLogoPreview));
    }

    private void AutoSave()
    {
        if (_isReloading) return;

        var prefs = _preferencesService.Current;
        prefs.SelectedTheme = SelectedTheme;
        prefs.DefaultOutputDirectory = IsCustomOutputDirectory ? CustomOutputDirectory : null;
        prefs.LogoPath = LogoPath;
        prefs.OverwriteExisting = OverwriteExisting;
        prefs.PreScanMetadata = PreScanMetadata;
        prefs.ReducedMotion = ReducedMotion;

        _preferencesService.Save(prefs);
    }

    [RelayCommand]
    private async Task PickFolderAsync()
    {
        var folder = await _storageService.PickFolderAsync();
        if (!string.IsNullOrEmpty(folder))
        {
            CustomOutputDirectory = folder;
            IsCustomOutputDirectory = true;
            AutoSave();
        }
    }

    [RelayCommand]
    private async Task PickLogoAsync()
    {
        var imagePath = await _storageService.PickImageFileAsync();
        if (!string.IsNullOrEmpty(imagePath))
        {
            LogoPath = imagePath;
            AutoSave();
        }
    }

    [RelayCommand]
    private void ClearLogo()
    {
        LogoPath = null;
        AutoSave();
    }

    [RelayCommand]
    private async Task SaveAsync()
    {
        var prefs = _preferencesService.Current;
        prefs.SelectedTheme = SelectedTheme;
        prefs.DefaultOutputDirectory = IsCustomOutputDirectory ? CustomOutputDirectory : null;
        prefs.LogoPath = LogoPath;
        prefs.OverwriteExisting = OverwriteExisting;
        prefs.PreScanMetadata = PreScanMetadata;
        prefs.ReducedMotion = ReducedMotion;

        await _preferencesService.SaveAsync(prefs);
        _toastService.Success("Preferencias Guardadas", "La configuración ha sido persistida exitosamente.");
    }
}
