using System;

namespace AppUI.Core.Services;

public enum NavigationTarget
{
    Conversion,
    Settings,
    Diagnostics
}

public interface INavigationService
{
    NavigationTarget CurrentTarget { get; }
    event Action<NavigationTarget>? Navigated;
    void NavigateTo(NavigationTarget target);
}
