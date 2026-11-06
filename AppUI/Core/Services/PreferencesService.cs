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
    private readonly object _lock = new();
    private AppPreferences _current = new();

    public event Action<AppPreferences>? PreferencesChanged;

    public PreferencesService()
    {
        var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        var dir = Path.Combine(appData, "XPressPDF");
        Directory.CreateDirectory(dir);
        _filePath = Path.Combine(dir, "preferences.json");
        LoadInternal();
    }

    public AppPreferences Current => _current;

    private void LoadInternal()
    {
        lock (_lock)
        {
            try
            {
                if (File.Exists(_filePath))
                {
                    var json = File.ReadAllText(_filePath);
                    var loaded = JsonSerializer.Deserialize<AppPreferences>(json, JsonOptions);
                    if (loaded != null)
                    {
                        _current = loaded;
                        return;
                    }
                }
            }
            catch
            {
                // Fallback a preferencias predeterminadas en caso de error
            }

            _current = new AppPreferences();
        }
    }

    public Task<AppPreferences> LoadAsync()
    {
        LoadInternal();
        return Task.FromResult(_current);
    }

    public void Save(AppPreferences preferences)
    {
        ArgumentNullException.ThrowIfNull(preferences);
        lock (_lock)
        {
            _current = preferences;
            try
            {
                var json = JsonSerializer.Serialize(preferences, JsonOptions);
                File.WriteAllText(_filePath, json);
            }
            catch
            {
                // Evitar fallas si el archivo está temporalmente bloqueado
            }
        }

        PreferencesChanged?.Invoke(_current);
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
            // Evitar interrupción si el disco está temporalmente bloqueado
        }

        PreferencesChanged?.Invoke(_current);
    }
}
