using System.Threading.Tasks;
using AppUI.Core.Models;

namespace AppUI.Core.Services;

using System;

public interface IPreferencesService
{
    AppPreferences Current { get; }
    event Action<AppPreferences>? PreferencesChanged;
    Task<AppPreferences> LoadAsync();
    Task SaveAsync(AppPreferences preferences);
    void Save(AppPreferences preferences);
}
