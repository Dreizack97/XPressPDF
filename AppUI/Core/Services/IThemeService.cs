using System;
using Avalonia.Styling;

namespace AppUI.Core.Services;

public interface IThemeService
{
    ThemeVariant CurrentTheme { get; }
    string CurrentThemeName { get; }
    event Action<ThemeVariant>? ThemeChanged;
    void SetTheme(string themeName);
    void SetTheme(ThemeVariant themeVariant);
    void ToggleTheme();
}
