using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;

namespace WebApp.Layout;

/// <summary>
/// Base class for layout components providing common functionality like mobile menu state and navigation handling.
/// </summary>
public partial class LayoutBase : LayoutComponentBase, IDisposable
{
    [Inject]
    protected NavigationManager Navigation { get; set; } = default!;

    /// <summary>
    /// Tracks whether the mobile menu (sidebar) is currently open.
    /// Protected so derived layouts can access and modify this state.
    /// </summary>
    protected bool IsMobileMenuOpen { get; set; } = false;

    /// <summary>
    /// Toggles the mobile menu open/closed state.
    /// Called by navbar toggle button or other UI elements.
    /// </summary>
    protected void ToggleMobileMenu()
    {
        IsMobileMenuOpen = !IsMobileMenuOpen;
        StateHasChanged();
    }

    /// <summary>
    /// Lifecycle method called when the component is initialized.
    /// Subscribes to navigation events to auto-close mobile menu on navigation.
    /// </summary>
    protected override void OnInitialized()
    {
        Navigation.LocationChanged += OnLocationChanged;
    }

    /// <summary>
    /// Event handler for navigation changes.
    /// Automatically closes the mobile menu when navigating to a new page.
    /// </summary>
    private void OnLocationChanged(object? sender, LocationChangedEventArgs e)
    {
        IsMobileMenuOpen = false;
        StateHasChanged();
    }

    /// <summary>
    /// Cleanup method called when the component is disposed.
    /// Unsubscribes from navigation events to prevent memory leaks.
    /// </summary>
    public void Dispose()
    {
        Navigation.LocationChanged -= OnLocationChanged;
    }
}
