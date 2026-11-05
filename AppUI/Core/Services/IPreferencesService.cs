using System.Threading.Tasks;
using AppUI.Core.Models;

namespace AppUI.Core.Services;

public interface IPreferencesService
{
    AppPreferences Current { get; }
    Task<AppPreferences> LoadAsync();
    Task SaveAsync(AppPreferences preferences);
}
