using System;
using Avalonia;
using Avalonia.Styling;

namespace AppUI.Core.Services;

public sealed class ThemeService : IThemeService
{
    public ThemeVariant CurrentTheme => Application.Current?.RequestedThemeVariant ?? ThemeVariant.Default;

    public string CurrentThemeName
    {
        get
        {
            if (CurrentTheme == ThemeVariant.Light) return "Light";
            if (CurrentTheme == ThemeVariant.Dark) return "Dark";
            return "System";
        }
    }

    public event Action<ThemeVariant>? ThemeChanged;

    public void SetTheme(string themeName)
    {
        var variant = themeName.ToLowerInvariant() switch
        {
            "light" => ThemeVariant.Light,
            "dark" => ThemeVariant.Dark,
            _ => ThemeVariant.Default
        };

        SetTheme(variant);
    }

    public void SetTheme(ThemeVariant themeVariant)
    {
        if (Application.Current != null && Application.Current.RequestedThemeVariant != themeVariant)
        {
            Application.Current.RequestedThemeVariant = themeVariant;
            ThemeChanged?.Invoke(themeVariant);
        }
    }

    public void ToggleTheme()
    {
        var current = Application.Current?.ActualThemeVariant ?? ThemeVariant.Light;
        var next = current == ThemeVariant.Dark ? ThemeVariant.Light : ThemeVariant.Dark;
        SetTheme(next);
    }
}
