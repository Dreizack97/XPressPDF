using System;

namespace AppUI.Core.Services;

public sealed class NavigationService : INavigationService
{
    private NavigationTarget _currentTarget = NavigationTarget.Conversion;

    public NavigationTarget CurrentTarget => _currentTarget;

    public event Action<NavigationTarget>? Navigated;

    public void NavigateTo(NavigationTarget target)
    {
        if (_currentTarget != target)
        {
            _currentTarget = target;
            Navigated?.Invoke(target);
        }
    }
}
