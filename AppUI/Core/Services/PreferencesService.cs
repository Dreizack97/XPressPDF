using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using AppUI.Core.Models;

namespace AppUI.Core.Services;

public sealed class PreferencesService : IPreferencesService
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true,
        PropertyNameCaseInsensitive = true
    };

    private readonly string _filePath;
    private AppPreferences _current = new();

    public PreferencesService()
    {
        var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        var dir = Path.Combine(appData, "XPressPDF");
        Directory.CreateDirectory(dir);
        _filePath = Path.Combine(dir, "preferences.json");
    }

    public AppPreferences Current => _current;

    public async Task<AppPreferences> LoadAsync()
    {
        try
        {
            if (File.Exists(_filePath))
            {
                var json = await File.ReadAllTextAsync(_filePath).ConfigureAwait(false);
                var loaded = JsonSerializer.Deserialize<AppPreferences>(json, JsonOptions);
                if (loaded != null)
                {
                    _current = loaded;
                    return _current;
                }
            }
        }
        catch
        {
            // Fallback a preferencias predeterminadas en caso de archivo corrupto o inaccesible
        }

        _current = new AppPreferences();
        return _current;
    }

    public async Task SaveAsync(AppPreferences preferences)
    {
        ArgumentNullException.ThrowIfNull(preferences);
        _current = preferences;

        try
        {
            var json = JsonSerializer.Serialize(preferences, JsonOptions);
            await File.WriteAllTextAsync(_filePath, json).ConfigureAwait(false);
        }
        catch
        {
            // No interrumpir la aplicación si el disco está temporalmente bloqueado
        }
    }
}
